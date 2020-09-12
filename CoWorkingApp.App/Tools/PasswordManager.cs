using System;
namespace CoWorkingApp.App.Tools{

    public static class PasswordManager
    {
        public static string GetPassWord()
        {
            string passwordInput = "";
            while(true)
            {
                var keyPress = Console.ReadKey(true);

                if(keyPress.Key == ConsoleKey.Enter){
                    Console.WriteLine(" ");
                    break;
                }
                else
                {
                    Console.Write("*");
                    passwordInput+=keyPress.KeyChar;
                }
            }

            return passwordInput;
        }
    }

}


