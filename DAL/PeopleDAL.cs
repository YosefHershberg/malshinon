using Models;
using Utils;

namespace DAL
{
    public static class PeopleDAL
    {
        public static List<Person> GetAllPeople()
        {
            string sql = "SELECT * FROM People";
            List<Dictionary<string, object?>> responseFromDB = DBConnection.Execute(sql);
            List<Person> people = responseFromDB.Select(row => new Person(
                Convert.ToInt32(row["Id"]),
                row["FullName"]?.ToString(),
                row["SecretCode"]?.ToString(),
                Convert.ToDateTime(row["CreatedAt"])
            )).ToList();
            return people;

        }

        public static int GetOrCreatePerson(string nameOrCode)
        {
            int personId = GetPersonIdByName(nameOrCode);
            if (personId != -1)
            {
                // Person already exists
                Logger.Log($"NewPerson: Found existing person {nameOrCode} with ID {personId}");
                return personId;
            }

            int codeId = GetPersonIdByCodeName(nameOrCode);
            if (codeId != -1)
            {
                // Person exists by code
                Logger.Log($"NewPerson: Found existing person {nameOrCode} with ID {codeId}");
                return codeId;
            }

            // Create new
            string code = GenerateSecretCode();
            string sql = $"INSERT INTO People (FullName, SecretCode) VALUES ('{nameOrCode.Replace("'", "''")}', '{code}')";
            DBConnection.Execute(sql);

            // Retrieve the new person's ID
            sql = $"SELECT Id FROM People WHERE SecretCode='{code}'";
            var result = DBConnection.Execute(sql);
            Logger.Log($"NewPerson: Created {nameOrCode} with code {code}");
            return Convert.ToInt32(result[0]["Id"]!);
        }

        public static int GetPersonIdByName(string name)
        {
            string sql = $"SELECT Id FROM People WHERE FullName='{name.Replace("'", "''")}'";
            var result = DBConnection.Execute(sql);
            if (result.Count == 0) return -1;
            return Convert.ToInt32(result[0]["Id"]!);
        }

        public static int GetPersonIdByCodeName(string codeName)
        {
            string sql = $"SELECT Id FROM People WHERE SecretCode='{codeName.Replace("'", "''")}'";
            var result = DBConnection.Execute(sql);
            if (result.Count == 0) return -1;
            return Convert.ToInt32(result[0]["Id"]!);
        }
        private static string GenerateSecretCode()
        {
            var guid = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
            return guid;
        }
        public static string? GetSecretCodeByName(string name)
        {
            string sql = $"SELECT SecretCode FROM People WHERE FullName='{name.Replace("'", "''")}'";
            var result = DBConnection.Execute(sql);
            if (result.Count == 0) return null;
            return result[0]["SecretCode"]?.ToString();
        }

        // Returns a single person by Id, or null if not found
        public static Person? GetPersonById(int id)
        {
            string sql = $"SELECT * FROM People WHERE Id={id}";
            var result = DBConnection.Execute(sql);
            if (result.Count == 0) return null;
            var row = result[0];
            return new Person(
                Convert.ToInt32(row["Id"]),
                row["FullName"]?.ToString(),
                row["SecretCode"]?.ToString(),
                Convert.ToDateTime(row["CreatedAt"]) 
            );
        }

        // Finds people where FullName contains the provided substring (case-sensitive per DB collation)
        public static List<Person> FindPeopleByNameLike(string partialName)
        {
            string safe = partialName.Replace("'", "''");
            string sql = $"SELECT * FROM People WHERE FullName LIKE '%{safe}%'";
            var rows = DBConnection.Execute(sql);
            return rows.Select(row => new Person(
                Convert.ToInt32(row["Id"]),
                row["FullName"]?.ToString(),
                row["SecretCode"]?.ToString(),
                Convert.ToDateTime(row["CreatedAt"]) 
            )).ToList();
        }

        // Updates a person's FullName by Id; returns true if the new name persisted
        public static bool UpdatePersonName(int id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) return false;
            string safe = newName.Replace("'", "''");
            string sql = $"UPDATE People SET FullName='{safe}' WHERE Id={id}";
            DBConnection.Execute(sql);

            // Verify the update
            sql = $"SELECT FullName FROM People WHERE Id={id}";
            var result = DBConnection.Execute(sql);
            bool ok = result.Count > 0 && string.Equals(result[0]["FullName"]?.ToString(), newName, StringComparison.Ordinal);
            if (ok)
                Logger.Log($"PersonUpdated: Id {id} -> '{newName}'");
            return ok;
        }
    }

}
