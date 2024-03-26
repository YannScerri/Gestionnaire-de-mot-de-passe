///ETML
///Auteur : Yann SCERRI
///Date : 19.03.2024
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                    {
                        case '1':
                            Console.Clear();
                            Console.WriteLine("liste des mots de passe existants :\n");
                            DisplayPasswords();

                            //Console.WriteLine("Appuyez sur la touche [Backspace] pour revenir au menu précédent");
                            
                            break;

                        case '2':
                            Console.Clear();
                            Console.WriteLine("Veuillez entrer le mot de passe que vous souhaitez ajouter");
                            //Console.WriteLine("Nom : ");
                            //string websiteName = Console.ReadLine();
                            string addPassword;
                            while (!string.IsNullOrEmpty(addPassword = Console.ReadLine()))
                            {
                                CreateFile(addPassword);
                                Console.WriteLine("Mot de passe ajouté avec succès");
                                Console.WriteLine("Vous pouvez ajouter un autre mot de passe en l'écrivant puis en pressant [Enter] à nouveau");
                            }
                            break;
                            

                            

                        case '3':
                            Console.Clear();
                            Console.WriteLine("Quel mot de passe souhaitez vous supprimer parmi ceux présents dans cette liste ?");
                            DisplayPasswords();
                            string removePassword;
                            while (!string.IsNullOrEmpty(removePassword = Console.ReadLine()))
                            {
                                DeletePasswords(removePassword);
                                Console.WriteLine("Vous pouvez supprimer un autre mot de passe en l'écrivant puis en pressant [Enter] à nouveau");
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
        static void CreateFile(string password)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //définit l'emplacement sur le bureau
            string passwordFolderPath = Path.Combine(desktopPath, "Password"); //définit le nom du dossier
            string fileName = $"password_{DateTime.Now:yyyyMMddHHmmss}.txt"; //définit le nom du fichier txt selon un format temporel
            string filePath = Path.Combine(passwordFolderPath, fileName);   // 

            try
            {   
                if (!Directory.Exists(passwordFolderPath))
                {
                    Directory.CreateDirectory(passwordFolderPath); //création du dossier
                }
                // Écriture du contenu dans le fichier
                File.WriteAllText(filePath, password);
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
                    foreach (string file in files)
                    {
                        string password = File.ReadAllText(file);
                        Console.WriteLine($"-{password}");
                    }
                }
                else
                {
                    Console.WriteLine("le dossier recherché n'existe pas");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture des fichiers de mot de passe : {ex.Message}");
            }
        }


        static void DeletePasswords(string password)
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
                        string filePassword = File.ReadAllText(file);
                        if(filePassword == password)
                        {
                            File.Delete(file);
                            Console.WriteLine($"Mot de passe {password} supprimé avec succès");
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

    }
}
    
