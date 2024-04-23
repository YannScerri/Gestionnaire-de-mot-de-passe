///ETML
///Auteur : Yann SCERRI
///Date de création : 19.03.2024
///Description : application de chiffrement de mots de passe
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Gestionnaire_de_mot_de_passe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            

            {   
                string userMasterPassword = "";
                
                


                Console.WriteLine("Veuillez saisir un master password puis pressez [Enter]");
                userMasterPassword = Console.ReadLine();
                Console.Clear();
                // Permet d'attribuer les touches du clavier aux cases
                ConsoleKeyInfo keyShowMenu = Console.ReadKey();

                if(keyShowMenu.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("***********************************************\n" +
                        "GESTIONNAIRE DE MOTS DE PASSE\n\n" +
                        "Sélectionnez une action via la touche correspondante\n" +
                        "[1] Consulter un mot de passe\n" +
                        "[2] Ajouter un mot de passe\n" +
                        "[3] Supprimer un mot de passe \n" +
                        "[4] Quitter le programme\n" +
                        "***********************************************\n\n" +
                        "Faites votre choix :");



                    ConsoleKeyInfo keyMenu = Console.ReadKey();
                    switch (keyMenu.KeyChar)
                    {   //liste de mots de passe
                        case '1':
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("liste des mots de passe existants :\n");
                                DisplayPasswords();
                                
                                if (Console.ReadKey().Key == ConsoleKey.Backspace)
                                    break;
                            }
                            

                            
                            
                            break;
                        //ajouter des mots de passe
                        case '2':
                            Console.Clear();
                            Console.WriteLine("Veuillez entrer le nom du site lié à votre mot de passe");
                            string websiteName = Console.ReadLine();
                            Console.WriteLine("Veuillez entrer l'URL du site");
                            string url = Console.ReadLine();
                            Console.WriteLine("Veuillez entrer votre identifiant");
                            string username = Console.ReadLine();
                            Console.WriteLine("Veuillez entrer le mot de passe que vous souhaitez ajouter :");
                            string addPassword;
                            while (!string.IsNullOrEmpty(addPassword = Console.ReadLine()))
                            {
                                CreateFile(websiteName, url, username, addPassword);
                                Console.WriteLine("Mot de passe ajouté avec succès");
                                Console.WriteLine("Vous pouvez changer le mot de passe en cas d'erreur en le réécrivant puis en pressant [Enter] à nouveau");
                            }
                            break;
                            

                            
                        //supprimer des mots de passes
                        case '3':
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
                            break;
                        

                        case '4':
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Veuillez entrez un choix valide (1-4)");
                            break;
                    }
                }
                

                

                













                Console.ReadLine();
            }
        }
        /// <summary>
        /// méthode qui permet de créer les fichier txt contenant les mdp
        /// </summary>
        /// <param name="password"></param>
        static void CreateFile(string websiteName, string url, string username, string password)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //définit l'emplacement sur le bureau
            string passwordFolderPath = Path.Combine(desktopPath, "Password"); //définit le nom du dossier
            string fileName = $"{websiteName}.txt"; //définit le nom du fichier txt selon un format temporel
            string filePath = Path.Combine(passwordFolderPath, fileName);   // 

            try
            {   
                if (!Directory.Exists(passwordFolderPath))
                {
                    Directory.CreateDirectory(passwordFolderPath); //création du dossier
                }
                // Écriture du contenu dans le fichier
                string fileContent = $"Site: {websiteName}\nURL: {url}\nIdentifiant : {username}\nMot de passe: {password}";
                File.WriteAllText(filePath, fileContent);
                Console.WriteLine("Fichier texte créé avec succès dans le dossier password.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du fichier : {ex.Message}");
            }
        }

        /// <summary>
        /// méthode qui permet d'afficher les mdp dans le dossier password
        /// </summary>
        static void DisplayPasswords()
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
                        Console.WriteLine($"\nContenu du fichier '{Path.GetFileName(selectedFilePath)}':\n{passwordContent}");
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
        }


        static void DeletePasswords(string websiteName = null, string url = null,string username = null, string password = null)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string passwordFolderPath = Path.Combine(desktopPath, "Password");

            try
            {
                if (Directory.Exists(passwordFolderPath))
                {
                    string[] files = Directory.GetFiles(passwordFolderPath);
                    foreach(string file in files)
                    {
                        string fileContent = File.ReadAllText(file);
                        if((websiteName != null && fileContent.Contains($"Site: {websiteName}")) || 
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
            catch(Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression du mot de passe : {ex.Message}");
            }

        }

        static string EncryptPassword(string password, string masterPassword)
        {
            // Convertir le master password en une clé de décalage
            int shift = CalculateShift(masterPassword);

            // Crypter le mot de passe en utilisant le décalage
            StringBuilder encryptedPassword = new StringBuilder();
            foreach (char letter in password)
            {
                if (char.IsLetter(letter))
                {
                    char encryptedLetter = (char)(letter + shift);
                    encryptedPassword.Append(encryptedLetter);
                }
                else
                {
                    encryptedPassword.Append(letter); // Ne pas crypter les caractères spéciaux, chiffres, etc.
                }
            }

            return encryptedPassword.ToString();
        }

        static int CalculateShift(string masterPassword) 
        {
            // Calculer un décalage à partir du master password (par exemple, la somme des codes ASCII des caractères)
            int shift = 0;
            foreach (char c in masterPassword)
            {
                shift += (int)c;
            }

            // Normaliser le décalage pour qu'il soit dans la plage des caractères ASCII imprimables
            shift = shift % 94; // 94 est le nombre de caractères imprimables dans ASCII (de l'espace à ~)

            return shift;
        }

    }
}
    
