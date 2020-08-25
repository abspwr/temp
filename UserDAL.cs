using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Administracija_Korisnika
{
    class UserDAL
    {
        public static List<User> FetchUsers() // Kreiranje metode za punjenje ListBox iz baze
        {
            List<User> userList = new List<User>(); // Lista za cuvanje korisnika iz ListBox

            SqlConnection Connect = new SqlConnection(Connection.cnn); // Kreiranje objekta konekcije sa bazom

            try // Dobro je uvijek koristiti Try / Catch za komunikaciju sa bazom radi gresaka koje se desavaju
            {
                using (Connect) // unutar Try blocka se koristi Using, jer ova naredba zatvara veze kod izlaska iz aplikacije
                {
                    Connect.Open(); // uvijek se prvo otvara konekcija
                    SqlCommand Command = new SqlCommand("SELECT * FROM Users", Connect); // Naredba za davanje Sql naredbi

                    User aUser = null; // Deklarisanje objekta za punjenje podataka

                    using (SqlDataReader r = Command.ExecuteReader()) // Nova using direktiva za citanje podataka pomocu SqlDataReader
                    {
                        while (r.Read()) // While se koristi kako bi petlja iscitala sve podatke iz baze
                        {
                            aUser = new User(); // Instanciranje objekta u koji ce se puniti podaci

                            aUser.UserId = Convert.ToInt32(r["Id"]); // Iscitavanje pojedinacnih atributa, uz obaveznu konverziju
                            aUser.UserName = r["UserName"].ToString();
                            aUser.UserPassword = r["UserPass"].ToString();
                            aUser.IsAdmin = Convert.ToInt32(r["IsAdmin"]);

                            userList.Add(aUser); //Dodavanje instanciranog objekta sa atributima u kreiranu listu
                        }
                    }
                }
            return userList;
            }
            catch (Exception)
            {
                return null; // Vracamo null da se lista ne bi napunila u slucaju greske
            }
        }

        public static int AddUser(User aUser)
        {
            using (SqlConnection connection = new SqlConnection(Connection.cnn))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("DECLARE @Completed BIT EXECUTE sp_AddUsers @UserNameArg, @UserPassArg, @IsAdminArg, @Completed OUTPUT", connection);
                    
                    SqlParameter parametarUserName = new SqlParameter("@UserNameArg", System.Data.SqlDbType.NVarChar);
                    parametarUserName.Value = aUser.UserName;

                    SqlParameter parametarUserPassword = new SqlParameter("@UserPassArg", System.Data.SqlDbType.NVarChar);
                    parametarUserPassword.Value = aUser.UserPassword;

                    SqlParameter parametarIsAdmin = new SqlParameter("@IsAdminArg", System.Data.SqlDbType.Bit);
                    parametarIsAdmin.Value = aUser.IsAdmin;

                    command.Parameters.Add(parametarUserName);
                    command.Parameters.Add(parametarUserPassword);
                    command.Parameters.Add(parametarIsAdmin);

                    int completed = command.ExecuteNonQuery();
                    return completed;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
    }
}
