using System;
using System.IO;
using System.Text;

namespace Gestionnaire_de_mot_de_passe
{
    internal class Program
    {
        private const string VIGENEREKEY = "LisanAlGaib123"; // Clé pour le chiffrement de Vigenère
        private static string masterPasswordFilePath;

        static void Main(string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");
            masterPasswordFilePath = Path.Combine(passwordFolderPath, "masterpassword.txt");

            if (!Directory.Exists(passwordFolderPath))
            {
                Directory.CreateDirectory(passwordFolderPath);
                SetMasterPassword();
            }

            string userMasterPassword = ""; //saisie du mot de passe principal par l'utilisateur

            Console.WriteLine("Veuillez saisir le mot de passe principal puis appuyez sur [Enter]:");
            userMasterPassword = Console.ReadLine();
            Console.Clear();

            if (VerifyMasterPassword(userMasterPassword)) //si le mot de passe principal est correct, l'application continue
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

        static void SetMasterPassword()
        {
            Console.WriteLine("Aucun mot de passe principal trouvé. Veuillez définir un nouveau mot de passe principal :");
            string newMasterPassword = Console.ReadLine();
            string encryptedMasterPassword = EncryptText(newMasterPassword, VIGENEREKEY);
            File.WriteAllText(masterPasswordFilePath, encryptedMasterPassword);
            Console.WriteLine("Mot de passe principal défini avec succès.");
        }

        static bool VerifyMasterPassword(string inputPassword)
        {
            if (!File.Exists(masterPasswordFilePath))
            {
                Console.WriteLine("Erreur : fichier de mot de passe principal introuvable.");
                return false;
            }

            string encryptedMasterPassword = File.ReadAllText(masterPasswordFilePath);
            string decryptedMasterPassword = DecryptText(encryptedMasterPassword, VIGENEREKEY);
            return inputPassword == decryptedMasterPassword;
        }

        // Le reste de votre code reste inchangé ...

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
                    Console.WriteLine("Veuillez entrer un choix valide (1-4)");
                    break;
            }
        }

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
                            string encryptedContent = File.ReadAllText(selectedFilePath);
                            string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);
                            Console.WriteLine($"\nContenu du fichier '{Path.GetFileName(selectedFilePath)}':\n{decryptedContent}");
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
                    ModifyPasswords(websiteName: siteToModify);
                    break;
                case '2':
                    Console.Clear();
                    Console.WriteLine("Veuillez entrer l'URL du site dont vous souhaitez modifier le mot de passe :");
                    string urlToModify = Console.ReadLine();
                    ModifyPasswords(url: urlToModify);
                    break;
                case '3':
                    Console.Clear();
                    Console.WriteLine("Veuillez entrer l'identifiant dont vous souhaitez modifier le mot de passe :");
                    string usernameToModify = Console.ReadLine();
                    ModifyPasswords(username: usernameToModify);
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
            }
        }

        static void CreateFile(string websiteName, string url, string username, string password)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            if (!Directory.Exists(passwordFolderPath))
            {
                Directory.CreateDirectory(passwordFolderPath);
            }

            string passwordFilePath = Path.Combine(passwordFolderPath, $"{websiteName}.txt");

            string fileContent = $"Site: {websiteName}\nURL: {url}\nIdentifiant: {username}\nMot de passe: {password}";
            string encryptedContent = EncryptText(fileContent, VIGENEREKEY);

            File.WriteAllText(passwordFilePath, encryptedContent);
        }

        static void DeletePasswords(string websiteName = null, string url = null, string username = null, string password = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            if (Directory.Exists(passwordFolderPath))
            {
                string[] files = Directory.GetFiles(passwordFolderPath);

                foreach (var file in files)
                {
                    string encryptedContent = File.ReadAllText(file);
                    string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);

                    if ((!string.IsNullOrEmpty(websiteName) && decryptedContent.Contains($"Site: {websiteName}")) ||
                        (!string.IsNullOrEmpty(url) && decryptedContent.Contains($"URL: {url}")) ||
                        (!string.IsNullOrEmpty(username) && decryptedContent.Contains($"Identifiant: {username}")) ||
                        (!string.IsNullOrEmpty(password) && decryptedContent.Contains($"Mot de passe: {password}")))
                    {
                        File.Delete(file);
                        Console.WriteLine($"Fichier '{Path.GetFileName(file)}' supprimé.");
                    }
                }
            }
        }

        static void ModifyPasswords(string websiteName = null, string url = null, string username = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            if (Directory.Exists(passwordFolderPath))
            {
                string[] files = Directory.GetFiles(passwordFolderPath);

                foreach (var file in files)
                {
                    string encryptedContent = File.ReadAllText(file);
                    string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);

                    if ((!string.IsNullOrEmpty(websiteName) && decryptedContent.Contains($"Site: {websiteName}")) ||
                        (!string.IsNullOrEmpty(url) && decryptedContent.Contains($"URL: {url}")) ||
                        (!string.IsNullOrEmpty(username) && decryptedContent.Contains($"Identifiant: {username}")))
                    {
                        Console.WriteLine($"Fichier trouvé : {Path.GetFileName(file)}");
                        Console.WriteLine("Veuillez entrer les nouvelles informations :");
                        Console.WriteLine("Nouvel URL :");
                        string newUrl = Console.ReadLine();
                        Console.WriteLine("Nouvel identifiant :");
                        string newUsername = Console.ReadLine();
                        Console.WriteLine("Nouveau mot de passe :");
                        string newPassword = Console.ReadLine();

                        string newFileContent = $"Site: {websiteName}\nURL: {newUrl}\nIdentifiant: {newUsername}\nMot de passe: {newPassword}";
                        string newEncryptedContent = EncryptText(newFileContent, VIGENEREKEY);

                        File.WriteAllText(file, newEncryptedContent);
                        Console.WriteLine("Modification réussie.");
                    }
                }
            }
        }

        static string EncryptText(string input, string key)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char c = (char)(input[i] ^ key[i % key.Length]);
                result.Append(c);
            }
            return result.ToString();
        }

        static string DecryptText(string input, string key)
        {
            return EncryptText(input, key);
        }
    }
}
