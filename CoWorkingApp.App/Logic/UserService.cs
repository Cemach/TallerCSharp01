using System;
using CoWorking.App.Data;
using CoWorking.App.Data.Tools;
using CoWorkingApp.App.Enumerations;
using CoWorking.App.Models;
using System.Globalization;
using CoWorkingApp.Data;
using System.Linq;
using CoWorkingApp.App.Tools;

namespace CoWorkingApp.App.Logic
{
    public class UserService{
        private UserData userData {get;set;}
        private DeskData deskData {get;set;}
        private User user{get;set;}

        private ReservationData reservationData {get;set;}

        public UserService(User user, UserData userData, DeskData deskData){
            this.userData = userData;
            this.deskData = deskData;
            this.user = user;
            this.reservationData = new ReservationData();
        }

        public void ExecuteAction(AdminUser menuAdminUserSelected)
        {
            switch(menuAdminUserSelected)
            {
                case AdminUser.Add:
                
                User newUser = new User();

                Console.WriteLine("Escriba el nombre");
                newUser.Name = Console.ReadLine();
                Console.WriteLine("Escriba el apellido");
                newUser.LastName = Console.ReadLine();
                Console.WriteLine("Escriba el Email");
                newUser.Email = Console.ReadLine();
                Console.WriteLine("Escriba el password");
                newUser.PassWord = PasswordManager.GetPassWord();

                userData.CreateUser(newUser);

                Console.WriteLine("Usuario creado!");

                break;
                case AdminUser.Edit:
                Console.WriteLine("Escriba correo del usuario");
                var userFound = userData.FindUser(Console.ReadLine());
                while(userFound==null){
                    Console.WriteLine("Escriba correo del usuario");
                    userFound = userData.FindUser(Console.ReadLine());
                }


                Console.WriteLine("Escriba el nombre");
                userFound.Name = Console.ReadLine();
                Console.WriteLine("Escriba el apellido");
                userFound.LastName = Console.ReadLine();
                Console.WriteLine("Escriba el Email");
                userFound.Email = Console.ReadLine();
                Console.WriteLine("Escriba el password");
                userFound.PassWord = PasswordManager.GetPassWord();

                userData.EditUser(userFound);

                Console.WriteLine("Usuario Actualizado!");
                break;
                case AdminUser.Delete:
                Console.WriteLine("Escriba correo del usuario");
                var userFoundDelete = userData.FindUser(Console.ReadLine());
                while (userFoundDelete ==null){
                    Console.WriteLine("Escriba correo del usuario");
                    userFoundDelete = userData.FindUser(Console.ReadLine());
                }

                Console.WriteLine($"Esta seguro que desea eliminar a {userFoundDelete.Name} {userFoundDelete.LastName}, SI-NO?");

                if(Console.ReadLine() == "SI")
                {
                    userData.DeleteUser(userFoundDelete.UserId);
                }
                Console.WriteLine("Usuario eliminado con exito");

                break;
                case AdminUser.ChangePassword:
                Console.WriteLine("Escriba correo del usuario");
                var userFoundPassword = userData.FindUser(Console.ReadLine());
                while(userFoundPassword == null)
                {
                    Console.WriteLine("Escriba correo del usuario");
                    userFoundPassword = userData.FindUser(Console.ReadLine());
                }

                Console.WriteLine("Escriba el password");
                userFoundPassword.PassWord = PasswordManager.GetPassWord();
                userData.EditUser(userFoundPassword);
                Console.WriteLine("Usuario Editado!");
                break;
            }

        }
    
        public User LoginUser(bool isAdmin)
        {
            bool loginResult = false;

            while(!loginResult){
                Console.WriteLine("Ingrese usuario");
                var userLogin = Console.ReadLine();
                Console.WriteLine("Ingrese Constraseña");
                var passwordLogin = PasswordManager.GetPassWord();

                var userLogged = userData.Login(userLogin,passwordLogin, isAdmin);
                loginResult = userLogged!=null;

                if(!loginResult) Console.WriteLine("Usuario o contraseña no exitoso");
                else return userLogged;
            }

            return null;
                
        }
    
        public void ExecuteActionByUser(User user, MenuUser menuUserSelected)
        {
            switch(menuUserSelected)
            {
                case MenuUser.ReservarPuesto:
                var deskList = deskData.GetAvailableDesk();
                Console.WriteLine("Puestos disponibles:");
                foreach(var item in deskList){
                    Console.WriteLine($"{item.Number} - {item.Description}");
                }

                var newReservation = new Reservation();
                Console.WriteLine("Ingrese número del puesto");
                var deskFound = deskData.FindDesk(Console.ReadLine());
                while(deskFound ==null)
                {
                    Console.WriteLine("Ingrese número del puesto");
                    deskFound = deskData.FindDesk(Console.ReadLine());

                }
                
                newReservation.DeskId = deskFound.DeskId;

                var dateSelected = new DateTime();

                while(dateSelected.Year == 0001)
                {
                    Console.WriteLine("Ingrese la fecha de reserva (dd-mm-yyyy)");
                    DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy",null,DateTimeStyles.None,out dateSelected);

                }

                newReservation.ReservationDate = dateSelected;
                newReservation.UserId = user.UserId;
                reservationData.CreateReservation(newReservation);
                Console.WriteLine("Reservación creada!");

                break;
                case MenuUser.CancelarReserva:
                Console.WriteLine("Estas son las reservaciones disponibles");
                var userReservations = reservationData.GetReservationByUser(user.UserId).ToList();
                var deskUserList = deskData.GetAvailableDesk().ToList();
                int indexReservation = 1;

                foreach(var item in userReservations)
                {
                    Console.WriteLine($"{indexReservation} - {deskUserList.FirstOrDefault(p => p.DeskId ==item.DeskId).Number} - {item.ReservationDate.ToString("dd-MM-yyy")}");
                    indexReservation++;
                }
                var indexReservationSelected = 0;
                while(indexReservationSelected<1 || indexReservationSelected>indexReservation)
                {
                    Console.WriteLine("Ingrese el número de la reservación que desea eliminar");
                    indexReservationSelected = int.Parse(Console.ReadLine());
                }

                Console.WriteLine("index reservation selected {0}",indexReservationSelected);
                var reservationToDelete = userReservations[indexReservationSelected-1];

                reservationData.CancelReservation(reservationToDelete.ReservationId);
                Console.WriteLine("Reservación cancelada.");

                break;
                case MenuUser.HistorialReservars:
                Console.WriteLine("Tus Reservas");
                var historyReservationUser = reservationData.GetReservationHistoryByUser(user.UserId);
                var deskHistoryList = deskData.GetAvailableDesk().ToList();
                foreach(var item in historyReservationUser)
                {
                    Console.ForegroundColor  = item.ReservationDate > DateTime.Now ? ConsoleColor.Green : ConsoleColor.DarkGray;
                    Console.WriteLine($"{deskHistoryList.FirstOrDefault(p => p.DeskId == item.DeskId).Number} - {item.ReservationDate.ToString("dd-MM-yyyy")} {(item.ReservationDate > DateTime.Now ? "(Activa)" : "")}");
                    Console.ResetColor();
                }
                break;
                case MenuUser.ChangePassoword:
                Console.WriteLine("Escriba el password");
                user.PassWord = PasswordManager.GetPassWord();
                userData.EditUser(user);
                Console.WriteLine("Password actualizado.");
                break;
            }
        }
    }
}
