﻿using System;
using CoWorking.App.Data;
using CoWorkingApp.App.Enumerations;
using CoWorkingApp.App.Logic;
using CoWorking.App.Data.Tools;
using CoWorking.App.Models;
using CoWorkingApp.App.Tools;

namespace CoWorkingApp.App
{
    class Program
    {
        static User ActiveUser;
        static UserData UserDataService {get;set;} = new UserData();
        static DeskData DeskDataService {get;set;} = new DeskData();

        static UserService UserLogicService {get;set;} = new UserService(ActiveUser, UserDataService, DeskDataService);
        static DeskService DeskLogicService {get;set;} = new DeskService(DeskDataService);

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido al Coworking");
            Console.WriteLine("Seleccione el rol con el que desea ingresar:");
            Console.WriteLine();

            string roleSelected = string.Empty;

            while(roleSelected!="1" && roleSelected!="2")
            {
                Console.WriteLine("1 = Admin, 2 = Usuario");
                roleSelected = Console.ReadLine();
            }

            UserRole userRoleSelected = Enum.Parse<UserRole>(roleSelected);
            //Admin
            if(userRoleSelected == UserRole.Admin)
            {
                

                //Login before action
                ActiveUser = UserLogicService.LoginUser(true);
                SpinnerManager.Show();

                string menuAdminSelected = String.Empty;
                while(menuAdminSelected!="1" && menuAdminSelected!="2")
                {
                    Console.WriteLine("1 = Administración de puestos, 2 = Administración de usuários");
                    menuAdminSelected = Console.ReadLine();
                }

                if(Enum.Parse<MenuAdmin>(menuAdminSelected) == MenuAdmin.AdministracionPuestos)
                {
                    string menuPuestosSelected = String.Empty;
                    Console.WriteLine("Administración de Puestos");                  

                    while(menuPuestosSelected!="1" && menuPuestosSelected!="2" &&
                    menuPuestosSelected!="3" && menuPuestosSelected!="4")
                    {
                        Console.WriteLine("1 = Crear, 2 = Editar, 3 = Eliminar, 4 = Bloquear");
                        menuPuestosSelected = Console.ReadLine();
                    }
                    AdminPuestos menuAdminPuestosSelected = Enum.Parse<AdminPuestos>(menuPuestosSelected);

                    DeskLogicService.ExecuteAction(menuAdminPuestosSelected);

                    
                }
                else if(Enum.Parse<MenuAdmin>(menuAdminSelected) == MenuAdmin.AdministracionUsuarios)
                {
                    string menuUsuariosSelected = String.Empty;
                    Console.WriteLine("Administración de Usuarios");
                    //Console.WriteLine("1 = Crear, 2 = Editar, 3 = Eliminar, 4 = Cambiar la Contraseña");

                    while(menuUsuariosSelected!="1" && menuUsuariosSelected!="2" &&
                    menuUsuariosSelected!="3" && menuUsuariosSelected!="4")
                    {
                        Console.WriteLine("1 = Crear, 2 = Editar, 3 = Eliminar, 4 = Cambiar la Contraseña");
                        menuUsuariosSelected = Console.ReadLine();
                    }
                    AdminUser menuAdminUserSelected = Enum.Parse<AdminUser>(menuUsuariosSelected);  
                    
                    UserLogicService.ExecuteAction(menuAdminUserSelected);
                }
                
            }
            
            //Usuario
            else if(Enum.Parse<UserRole>(roleSelected) == UserRole.User)
            {

                //Login before action
                ActiveUser = UserLogicService.LoginUser(false);


                string menuUsuarioSelected = String.Empty;

                while(menuUsuarioSelected!="1" && menuUsuarioSelected!="2" && menuUsuarioSelected!="3" &&menuUsuarioSelected!="4"){
                    Console.WriteLine("1 = Reservar puesto, 2 = Cancelar reserva, 3 = Ver el Historial de reservas, 4= Cambiar contraseña");
                    menuUsuarioSelected = Console.ReadLine();
                }

                MenuUser menuUserSelected= Enum.Parse<MenuUser>(menuUsuarioSelected);

                UserLogicService.ExecuteActionByUser(ActiveUser, menuUserSelected);
                                
            }
        }
    
        
    }
}
