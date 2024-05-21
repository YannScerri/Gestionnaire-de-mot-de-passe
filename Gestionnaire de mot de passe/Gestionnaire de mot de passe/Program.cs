///ETML
///Auteur : Yann Scerri
///Date de début : 19.03.2024
///Description: Programme de gestion de mot de passe avec encryptage Vigenere
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gestionnaire_de_mot_de_passe
{
    internal class Program
    {
        private const string MASTERPASSWORD = "1234"; //mot de passe principal correct
        private const string VIGENEREKEY = "LisanAlGaib123"; // Clé pour le chiffrement de Vigenère

        static void Main(string[] args)
        {
            string userMasterPassword = ""; //saisie du mot de passe principal par l'utilisateur

            Console.WriteLine("Veuillez saisir le mot de passe principal puis appuyez sur [Enter]:");
            userMasterPassword = Console.ReadLine();
            Console.Clear();

            if (userMasterPassword == MASTERPASSWORD) //si le mot de passe principal est correct, l'application continue
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
                    Console.WriteLine("Veuillez entrer un choix valide (1-4)");
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
                    bool passwordFound = false;

                    foreach (string file in files)
                    {
                        string encryptedContent = File.ReadAllText(file);
                        string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);

                        if (decryptedContent.Contains($"Site: {siteName}"))
                        {
                            passwordFound = true;
                            Console.WriteLine("Mot de passe actuel :");
                            Console.WriteLine(decryptedContent);
                            Console.WriteLine("Veuillez entrer le nouveau mot de passe :");
                            string newPassword = Console.ReadLine();

                            string updatedContent = decryptedContent.Replace(decryptedContent, newPassword);
                            string encryptedUpdatedContent = EncryptText(updatedContent, VIGENEREKEY);
                            File.WriteAllText(file, encryptedUpdatedContent);

                            Console.WriteLine("Mot de passe modifié avec succès.");
                            break;
                        }
                    }

                    if (!passwordFound)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé pour le site spécifié.");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier recherché n'existe pas.");
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
                    bool passwordFound = false;

                    foreach (string file in files)
                    {
                        string encryptedContent = File.ReadAllText(file);
                        string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);

                        if (decryptedContent.Contains($"URL: {url}"))
                        {
                            passwordFound = true;
                            Console.WriteLine("Mot de passe actuel :");
                            Console.WriteLine(decryptedContent);
                            Console.WriteLine("Veuillez entrer le nouveau mot de passe :");
                            string newPassword = Console.ReadLine();

                            string updatedContent = decryptedContent.Replace(decryptedContent, newPassword);
                            string encryptedUpdatedContent = EncryptText(updatedContent, VIGENEREKEY);
                            File.WriteAllText(file, encryptedUpdatedContent);

                            Console.WriteLine("Mot de passe modifié avec succès.");
                            break;
                        }
                    }

                    if (!passwordFound)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé pour l'URL spécifiée.");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier recherché n'existe pas.");
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
                    bool passwordFound = false;

                    foreach (string file in files)
                    {
                        string encryptedContent = File.ReadAllText(file);
                        string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);

                        if (decryptedContent.Contains($"Identifiant: {username}"))
                        {
                            passwordFound = true;
                            Console.WriteLine("Mot de passe actuel :");
                            Console.WriteLine(decryptedContent);
                            Console.WriteLine("Veuillez entrer le nouveau mot de passe :");
                            string newPassword = Console.ReadLine();

                            string updatedContent = decryptedContent.Replace(decryptedContent, newPassword);
                            string encryptedUpdatedContent = EncryptText(updatedContent, VIGENEREKEY);
                            File.WriteAllText(file, encryptedUpdatedContent);

                            Console.WriteLine("Mot de passe modifié avec succès.");
                            break;
                        }
                    }

                    if (!passwordFound)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé pour l'identifiant spécifié.");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier recherché n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du mot de passe : {ex.Message}");
            }
        }

        static void DeletePasswords(string websiteName = null, string url = null, string username = null, string password = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            try
            {
                if (Directory.Exists(passwordFolderPath))
                {
                    string[] files = Directory.GetFiles(passwordFolderPath);
                    bool passwordFound = false;

                    foreach (string file in files)
                    {
                        string encryptedContent = File.ReadAllText(file);
                        string decryptedContent = DecryptText(encryptedContent, VIGENEREKEY);

                        if ((websiteName != null && decryptedContent.Contains($"Site: {websiteName}")) ||
                            (url != null && decryptedContent.Contains($"URL: {url}")) ||
                            (username != null && decryptedContent.Contains($"Identifiant: {username}")) ||
                            (password != null && decryptedContent.Contains($"Mot de passe: {password}")))
                        {
                            File.Delete(file);
                            passwordFound = true;
                            Console.WriteLine($"Mot de passe pour '{Path.GetFileName(file)}' supprimé avec succès.");
                            break;
                        }
                    }

                    if (!passwordFound)
                    {
                        Console.WriteLine("Aucun mot de passe trouvé pour les critères spécifiés.");
                    }
                }
                else
                {
                    Console.WriteLine("Le dossier recherché n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression des mots de passe : {ex.Message}");
            }
        }

        /// <summary>
        /// Méthode pour créer le fichier du mdp et l'ajouter dans le dossier Password
        /// </summary>
        /// <param name="websiteName">nom du site</param>
        /// <param name="url">url du site</param>
        /// <param name="username">nom d'utilisateur</param>
        /// <param name="password">mot de passe</param>
        static void CreateFile(string websiteName, string url, string username, string password)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");
            Directory.CreateDirectory(passwordFolderPath);

            string filePath = Path.Combine(passwordFolderPath, $"{websiteName}.txt");

            string fileContent = $"Site: {websiteName}\nURL: {url}\nIdentifiant: {username}\nMot de passe: {password}";
            string encryptedContent = EncryptText(fileContent, VIGENEREKEY);

            File.WriteAllText(filePath, encryptedContent);
        }

        /// <summary>
        /// Méthode de chiffrement Vigenère
        /// </summary>
        /// <param name="text">texte à chiffrer</param>
        /// <param name="key">clé de chiffrement</param>
        /// <returns>texte chiffré</returns>
        static string EncryptText(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToUpper();

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (char.IsLetter(c))
                {
                    bool isUpper = char.IsUpper(c);
                    char offset = isUpper ? 'A' : 'a';
                    c = (char)((c + key[j % key.Length] - 2 * offset) % 26 + offset);
                    j++;
                }

                result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Méthode de déchiffrement Vigenère
        /// </summary>
        /// <param name="text">texte à déchiffrer</param>
        /// <param name="key">clé de déchiffrement</param>
        /// <returns>texte déchiffré</returns>
        static string DecryptText(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToUpper();

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (char.IsLetter(c))
                {
                    bool isUpper = char.IsUpper(c);
                    char offset = isUpper ? 'A' : 'a';
                    c = (char)((c - key[j % key.Length] + 26) % 26 + offset);
                    j++;
                }

                result.Append(c);
            }

            return result.ToString();
        }
    }
}
