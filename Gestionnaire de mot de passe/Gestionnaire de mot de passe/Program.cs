///ETML
///Auteur : Yann Scerri
///Date de début : 19.03.2024
///Description: Programme de gestion de mot de passe avec encryptage Vigenere
using System;
using System.Collections.Generic;
using System.IO;

namespace Gestionnaire_de_mot_de_passe
{
    internal class Program
    {   
        private const string MasterPassword = "1234"; //mot de passe principal correct
        private const int CaesarShift = 3; // Décalage pour le chiffrement de César

        static void Main(string[] args)
        {
            string userMasterPassword = ""; //saisie du mot de passe principal par l'utilisateur

            Console.WriteLine("Veuillez saisir le mot de passe principal puis appuyez sur [Enter]:");
            userMasterPassword = Console.ReadLine();
            Console.Clear();

            if (userMasterPassword == MasterPassword) //si le mot de passe principal est correct, l'application continue
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Accès autorisé.");
                Console.ResetColor();
                DisplayMenu();
            }
            else //sinon, l'accès est refusé
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Accès refusé. Mot de passe incorrect.");
                Console.ResetColor();
            }

            Console.ReadLine();
        }

        /// <summary>
        /// méthode qui affiche le menu principal
        /// </summary>
        static void DisplayMenu()
        {
            Console.WriteLine("***********************************************\n" +
                              "GESTIONNAIRE DE MOTS DE PASSE\n\n" +
                              "Sélectionnez une action via la touche correspondante\n" +
                              "[1] Consulter un mot de passe\n" +
                              "[2] Ajouter un mot de passe\n" +
                              "[3] Modifier un mot de passe\n" +
                              "[4] Supprimer un mot de passe \n" +
                              "[5] Quitter le programme\n" +
                              "***********************************************\n\n" +
                              "Faites votre choix :");

            ConsoleKeyInfo keyMenu = Console.ReadKey();
            switch (keyMenu.KeyChar) //switch pour les différentes options possibles
            {
                case '1':
                    Console.Clear();
                    DisplayPasswords(); //consulter les mdp existants
                    break;
                case '2':
                    AddPassword(); //ajouter un nouveau mdp
                    break;
                case '3':
                    ModifyPassword();
                    break;
                        
                case '4':
                    DeletePassword(); //supprimer un mdp
                    break;
                case '5':
                    Environment.Exit(0); //quitter l'application
                    break;
                default:
                    Console.WriteLine("Veuillez entrez un choix valide (1-4)");
                    break;
            }
        }
        /// <summary>
        /// méthode pour créer un nouveau mdp
        /// </summary>
        static void AddPassword()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Veuillez entrer le nom du site lié à votre mot de passe");
                string websiteName = Console.ReadLine();
                Console.WriteLine("Veuillez entrer l'URL du site");
                string url = Console.ReadLine();
                Console.WriteLine("Veuillez entrer votre identifiant");
                string username = Console.ReadLine();
                Console.WriteLine("Veuillez entrer le mot de passe que vous souhaitez ajouter :");
                string password = Console.ReadLine();

                CreateFile(websiteName, url, username, password);
                Console.WriteLine("Mot de passe ajouté avec succès");

                Console.WriteLine("\nAppuyez sur 'm' pour retourner au menu principal ou 'q' pour quitter :");
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.M)
                {
                    Console.Clear();
                    DisplayMenu();
                    break;
                }
            } while (true);

        }
        /// <summary>
        /// méthode pour afficher les mdp existants
        /// </summary>
        static void DisplayPasswords()
        {
            do
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string passwordFolderPath = Path.Combine(desktopPath, "Password");

                try
                {
                    if (Directory.Exists(passwordFolderPath))
                    {
                        string[] files = Directory.GetFiles(passwordFolderPath);

                        if (files.Length == 0)
                        {
                            Console.WriteLine("Aucun mot de passe enregistré.");
                            return;
                        }

                        Console.WriteLine("Fichiers de mots de passe disponibles :");

                        for (int i = 0; i < files.Length; i++)
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files[i]);
                            Console.WriteLine($"[{i + 1}] {fileNameWithoutExtension}");
                        }

                        Console.WriteLine("\nVeuillez entrer le numéro du fichier que vous souhaitez consulter et pressez [Enter] :");

                        if (int.TryParse(Console.ReadLine(), out int selectedFileIndex) && selectedFileIndex >= 1 && selectedFileIndex <= files.Length)
                        {
                            string selectedFilePath = files[selectedFileIndex - 1];
                            string passwordContent = File.ReadAllText(selectedFilePath);
                            string decryptedPassword = DecryptPassword(passwordContent);
                            Console.WriteLine($"\nContenu du fichier '{Path.GetFileName(selectedFilePath)}':\n{decryptedPassword}");
                        }
                        else
                        {
                            Console.WriteLine("Numéro de fichier invalide.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Le dossier recherché n'existe pas.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la lecture des fichiers de mot de passe : {ex.Message}");
                }
                Console.WriteLine("\nAppuyez sur 'm' pour revenir au menu principal");
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.M)
                {
                    Console.Clear();
                    DisplayMenu();
                    break;
                }
            } while (true);

        }
        /// <summary>
        /// méthode pour supprimer des mdp
        /// </summary>
        static void DeletePassword()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Veuillez choisir une option de suppression :");
                Console.WriteLine("[1] Supprimer par nom de site");
                Console.WriteLine("[2] Supprimer par URL");
                Console.WriteLine("[3] Supprimer par identifiant");
                Console.WriteLine("[4] Supprimer par mot de passe");

                ConsoleKeyInfo deleteOption = Console.ReadKey();

                switch (deleteOption.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine("Veuillez entrer le nom du site dont vous souhaitez supprimer le mot de passe :");
                        string siteToDelete = Console.ReadLine();
                        DeletePasswords(websiteName: siteToDelete);
                        break;
                    case '2':
                        Console.Clear();
                        Console.WriteLine("Veuillez entrer l'URL du site dont vous souhaitez supprimer le mot de passe :");
                        string urlToDelete = Console.ReadLine();
                        DeletePasswords(url: urlToDelete);
                        break;
                    case '3':
                        Console.Clear();
                        Console.WriteLine("Veuillez entrer l'identifiant dont vous souhaitez supprimer le mot de passe :");
                        string usernameToDelete = Console.ReadLine();
                        DeletePasswords(username: usernameToDelete);
                        break;
                    case '4':
                        Console.Clear();
                        Console.WriteLine("Veuillez entrer le mot de passe que vous souhaitez supprimer :");
                        string passwordToDelete = Console.ReadLine();
                        DeletePasswords(password: passwordToDelete);
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez choisir une option valide.");
                        break;


                }
                Console.WriteLine("\nAppuyez sur 'm' pour retourner au menu principal ou 'q' pour quitter :");
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.M)
                {
                    Console.Clear();
                    DisplayMenu();
                    break;
                }
            } while (true);

        }

        static void ModifyPassword()
        {
            Console.Clear();
            Console.WriteLine("Veuillez choisir une option de modification :");
            Console.WriteLine("[1] Modifier par nom de site");
            Console.WriteLine("[2] Modifier par URL");
            Console.WriteLine("[3] Modifier par identifiant");

            ConsoleKeyInfo modifyOption = Console.ReadKey();

            switch (modifyOption.KeyChar)
            {
                case '1':
                    Console.Clear();
                    Console.WriteLine("Veuillez entrer le nom du site dont vous souhaitez modifier le mot de passe :");
                    string siteToModify = Console.ReadLine();
                    ModifyPasswordBySite(siteToModify);
                    break;
                case '2':
                    Console.Clear();
                    Console.WriteLine("Veuillez entrer l'URL du site dont vous souhaitez modifier le mot de passe :");
                    string urlToModify = Console.ReadLine();
                    ModifyPasswordByUrl(urlToModify);
                    break;
                case '3':
                    Console.Clear();
                    Console.WriteLine("Veuillez entrer l'identifiant dont vous souhaitez modifier le mot de passe :");
                    string usernameToModify = Console.ReadLine();
                    ModifyPasswordByUsername(usernameToModify);
                    break;
                default:
                    Console.WriteLine("Option invalide. Veuillez choisir une option valide.");
                    break;
            }
        }

        static void ModifyPasswordBySite(string siteName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            try
            {
                if (Directory.Exists(passwordFolderPath))
                {
                    string[] files = Directory.GetFiles(passwordFolderPath);
                    bool passwordModified = false;

                    foreach (string file in files)
                    {
                        string fileContent = File.ReadAllText(file);
                        if (fileContent.Contains($"Site: {siteName}"))
                        {
                            Console.WriteLine($"Ancien contenu du fichier '{Path.GetFileName(file)}':\n{fileContent}");

                            Console.WriteLine("Veuillez entrer le nouveau mot de passe :");
                            string newPassword = Console.ReadLine();

                            string encryptedPassword = EncryptPassword(newPassword);

                            fileContent = fileContent.Replace($"Mot de passe: {DecryptPassword(encryptedPassword)}", $"Mot de passe: {encryptedPassword}");

                            File.WriteAllText(file, fileContent);
                            Console.WriteLine($"Mot de passe modifié avec succès pour le site '{siteName}'");
                            passwordModified = true;
                            break;
                        }
                    }

                    if (!passwordModified)
                    {
                        Console.WriteLine($"Aucun mot de passe trouvé pour le site '{siteName}'");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier ou le mot de passe n'existe pas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du mot de passe : {ex.Message}");
            }
        }

        static void ModifyPasswordByUrl(string url)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            try
            {
                if (Directory.Exists(passwordFolderPath))
                {
                    string[] files = Directory.GetFiles(passwordFolderPath);
                    bool passwordModified = false;

                    foreach (string file in files)
                    {
                        string fileContent = File.ReadAllText(file);
                        if (fileContent.Contains($"URL: {url}"))
                        {
                            Console.WriteLine($"Ancien contenu du fichier '{Path.GetFileName(file)}':\n{fileContent}");

                            Console.WriteLine("Veuillez entrer le nouveau mot de passe :");
                            string newPassword = Console.ReadLine();

                            string encryptedPassword = EncryptPassword(newPassword);

                            fileContent = fileContent.Replace($"Mot de passe: {DecryptPassword(encryptedPassword)}", $"Mot de passe: {encryptedPassword}");

                            File.WriteAllText(file, fileContent);
                            Console.WriteLine($"Mot de passe modifié avec succès pour l'URL '{url}'");
                            passwordModified = true;
                            break;
                        }
                    }

                    if (!passwordModified)
                    {
                        Console.WriteLine($"Aucun mot de passe trouvé pour l'URL '{url}'");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier ou le mot de passe n'existe pas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du mot de passe : {ex.Message}");
            }
        }

        static void ModifyPasswordByUsername(string username)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            try
            {
                if (Directory.Exists(passwordFolderPath))
                {
                    string[] files = Directory.GetFiles(passwordFolderPath);
                    bool passwordModified = false;

                    foreach (string file in files)
                    {
                        string fileContent = File.ReadAllText(file);
                        if (fileContent.Contains($"Identifiant : {username}"))
                        {
                            Console.WriteLine($"Ancien contenu du fichier '{Path.GetFileName(file)}':\n{fileContent}");

                            Console.WriteLine("Veuillez entrer le nouveau mot de passe :");
                            string newPassword = Console.ReadLine();

                            string encryptedPassword = EncryptPassword(newPassword);

                            fileContent = fileContent.Replace($"Mot de passe: {DecryptPassword(encryptedPassword)}", $"Mot de passe: {encryptedPassword}");

                            File.WriteAllText(file, fileContent);
                            Console.WriteLine($"Mot de passe modifié avec succès pour l'identifiant '{username}'");
                            passwordModified = true;
                            break;
                        }
                    }

                    if (!passwordModified)
                    {
                        Console.WriteLine($"Aucun mot de passe trouvé pour l'identifiant '{username}'");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier ou le mot de passe n'existe pas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du mot de passe : {ex.Message}");
            }
        }


        /// <summary>
        /// méthode permettant de créer des fichiers sur le bureau
        /// </summary>
        /// <param name="websiteName"></param>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        static void CreateFile(string websiteName, string url, string username, string password)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");
            string fileName = $"{websiteName}.txt";
            string filePath = Path.Combine(passwordFolderPath, fileName);

            try
            {
                if (!Directory.Exists(passwordFolderPath))
                {
                    Directory.CreateDirectory(passwordFolderPath);
                }

                
                string encryptedPassword = EncryptPassword(password);

                
                string fileContent = $"Site: {websiteName}\nURL: {url}\nIdentifiant : {username}\nMot de passe: {encryptedPassword}";
                File.WriteAllText(filePath, fileContent);
                Console.WriteLine("Fichier texte créé avec succès dans le dossier password.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du fichier : {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="websiteName"></param>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        static void DeletePasswords(string websiteName = null, string url = null, string username = null, string password = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            try
            {
                if (Directory.Exists(passwordFolderPath))
                {
                    string[] files = Directory.GetFiles(passwordFolderPath);
                    foreach (string file in files)
                    {
                        string fileContent = File.ReadAllText(file);
                        if ((websiteName != null && fileContent.Contains($"Site: {websiteName}")) ||
                           (url != null && fileContent.Contains($"URL: {url}")) ||
                           (username != null && fileContent.Contains($"Identifiant : {username}")) ||
                           (password != null && fileContent.Contains($"Mot de passe: {password}")))
                        {
                            File.Delete(file);
                            Console.WriteLine($"Fichier {Path.GetFileName(file)} supprimé avec succès");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier ou le mot de passe n'existe pas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression du mot de passe : {ex.Message}");
            }
        }

        static string EncryptPassword(string password)
        {
            // Encrypt password using Caesar cipher
            char[] encryptedChars = new char[password.Length];

            for (int i = 0; i < password.Length; i++)
            {
                char c = password[i];
                if (char.IsLetter(c))
                {
                    char encryptedChar = (char)(c + CaesarShift);
                    if (!char.IsLetter(encryptedChar))
                    {
                        encryptedChar = (char)(((encryptedChar - 'A') % 26) + 'A'); // Convert to uppercase letter
                    }
                    encryptedChars[i] = encryptedChar;
                }
                else
                {
                    encryptedChars[i] = c;
                }
            }

            return new string(encryptedChars);
        }

        static string DecryptPassword(string encryptedPassword)
        {
            
            char[] decryptedChars = new char[encryptedPassword.Length];

            for (int i = 0; i < encryptedPassword.Length; i++)
            {
                char c = encryptedPassword[i];
                if (char.IsLetter(c))
                {
                    char decryptedChar = (char)(c - CaesarShift);
                    if (!char.IsLetter(decryptedChar))
                    {
                        decryptedChar = (char)(((decryptedChar - 'A' + 26) % 26) + 'A'); //minuscule et majuscule
                    }
                    decryptedChars[i] = decryptedChar;
                }
                else
                {
                    decryptedChars[i] = c;
                }
            }

            return new string(decryptedChars);
        }
    }
}
