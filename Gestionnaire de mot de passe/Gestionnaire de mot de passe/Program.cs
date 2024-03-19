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
            ;
            

            {   //liste de choix possible de l'utilisateur
                string userMasterPassword = "";
                string addPassword = "";
                string removePassword = "";

                Console.WriteLine("Veuillez saisir un master password puis pressez [Enter]");
                userMasterPassword = Console.ReadLine();
                Console.Clear();
                // Permet d'attribuer les touches du clavier aux cases
                ConsoleKeyInfo keyShowMenu = Console.ReadKey();

                if(keyShowMenu.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("***********************************************\n" +
                        "Sélectionnez une action via la touche correspondante\n" +
                        "[1] Consulter un mot de passe\n" +
                        "[2] Ajouter un mot de passe\n" +
                        "[3] Supprimer un mot de passe \n" +
                        "[4] Quitter le programme\n" +
                        "***********************************************");



                    ConsoleKeyInfo keyMenu = Console.ReadKey();
                    switch (keyMenu.KeyChar)
                    {
                        case '1':
                            Console.Clear();
                            Console.WriteLine("liste des mots de passe existants :");
                            break;

                        case '2':
                            Console.Clear();
                            CreateFile();
                            //Console.WriteLine("Veuillez entrer le mot de passe que vous souhaitez ajouter");
                            //addPassword = Console.ReadLine();
                            break;

                        case '3':
                            Console.Clear();
                            Console.WriteLine("Quel mot de passe souhaitez vous supprimer ?");
                            removePassword = Console.ReadLine();
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
        static void CreateFile()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "exemple.txt");

            try
            {
                // Écriture du contenu dans le fichier
                File.WriteAllText(filePath, "salut");
                Console.WriteLine("Fichier texte créé avec succès sur le bureau.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du fichier : {ex.Message}");
            }
        }

        
    }
}
    
