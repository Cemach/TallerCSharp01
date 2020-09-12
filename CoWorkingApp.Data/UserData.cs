using System;
using System.Linq;
using CoWorking.App.Models;
using CoWorking.App.Data.Tools;

namespace CoWorking.App.Data
{
    public class UserData
    {
        private JsonManager<User> jsonManager;

        public UserData(){
            jsonManager = new JsonManager<User>();
        }

        public bool CreateAdmin(){

            var userCollection = jsonManager.GetCollection();

            if(!userCollection.Any(p=>p.Name == "ADMIN" &&p.LastName=="ADMIN" && p.Email=="ADMIN")){

                try{
                    var adminUser = new User(){
                    Name = "ADMIN",
                    LastName = "ADMIN",
                    Email = "ADMIN",
                    UserId = Guid.NewGuid(),
                    PassWord = EncryptData.EncrypText("4dmin!"),
                    };
                    userCollection.Add(adminUser);

                    jsonManager.SaveCollection(userCollection); 
                }
                catch{
                    return false;
                }
            return true;
        }
        return true;
        }
    
        public User Login(string User, string PassWord, bool isAdmin = false)
        {
            var userCollection = jsonManager.GetCollection();

            var passwordEncrypt = EncryptData.EncrypText(PassWord);

            if(isAdmin) User = "ADMIN";
            var userFound = userCollection.FirstOrDefault(p=> p.Email == User && p.PassWord == passwordEncrypt);

            //if(userFound != null) return true;

            return userFound;
        }

        public bool CreateUser(User newUser){

            newUser.PassWord = EncryptData.EncrypText(newUser.PassWord);
            var userCollection = jsonManager.GetCollection();

            userCollection.Add(newUser);

            jsonManager.SaveCollection(userCollection);

            return true;
        }

        public User FindUser(string email){
            var userCollection = jsonManager.GetCollection();

            return userCollection.FirstOrDefault(p=> p.Email == email);
        }

        public bool EditUser(User editUser){

            editUser.PassWord = EncryptData.EncrypText(editUser.PassWord);
            var userCollection = jsonManager.GetCollection();

            var indexUser = userCollection.FindIndex(p=> p.UserId==editUser.UserId);
            
            userCollection[indexUser]= editUser;

            jsonManager.SaveCollection(userCollection);

            return true;
        }

        public bool DeleteUser(Guid userId){
            var userCollection = jsonManager.GetCollection();
            userCollection.Remove(userCollection.Find(p=>p.UserId==userId));
            jsonManager.SaveCollection(userCollection);

            return true;
        }

    }
}
