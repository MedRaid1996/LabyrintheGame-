using System;
using System.Collections.Generic;
using System.Web;

class LabyrintheJeu
{
    // Déclaration des variables globales
    static char[,] grille; // Matrice représentant le labyrinthe
    static int taille = 18; // Taille fixe du labyrinthe
    static (int x, int y) joueur = (1, 1); // Position fixe du joueur (départ à chaque nouvelle tentative)
    static (int x, int y) sortie = (16, 16); // Position fixe de la sortie
    static List<(int x, int y)> cles = new List<(int, int)>(); // Liste des positions des clés
    static HashSet<(int, int)> clesRecuperees = new HashSet<(int, int)>(); // Ensemble des clés récupérées
    static Random rnd = new Random(); // Générateur de nombres aléatoires
    static int tentative = 1; // Compteur des tentatives

    static void Main()
    {
        while (true)
        {
            InitialiserLabyrinthe();
            while (true)
            {
                AfficherLabyrinthe();
                DeplacerJoueur();
                if (VerifVictoire())
                {
                    Celebration();
                    CleanKeyBoard();
                    if (tentative < 4) tentative++; // Augmenter le nombre de clés jusqu'à 4

                    break;
                }
            }
        }
    }

    static void InitialiserLabyrinthe()
    {
        // Création du labyrinthe avec murs et chemins
        grille = new char[taille, taille];
        cles.Clear(); // Réinitialisation des clés
        clesRecuperees.Clear(); // Réinitialisation des clés récupérées

        // Remise à zéro de la position du joueur à chaque nouvelle tentative
        joueur = (1, 1);

        //Deux boucles pour emplacer les murs
        for (int i = 0; i < taille; i++)
        {
            for (int j = 0; j < taille; j++)
            {
                if (i == 0 || j == 0 || i == taille - 1 || j == taille - 1 || rnd.Next(0, 6) == 0)
                {
                    grille[i, j] = '#'; // Placement aléatoire des murs
                }
                else
                {
                    grille[i, j] = ' '; //s'il y aura pas des mure donc on génere un espace qui represente une route
                }
            }
        }

        // Assurer un chemin vers la sortie 
        CreerChemin(joueur.x, joueur.y, sortie.x, sortie.y);

        // Assurer un chemin vers la sortie les clés
        foreach (var cle in cles)
        {
            CreerChemin(joueur.x, joueur.y, cle.x, cle.y);
        }

        // Placement du joueur et de la sortie
        grille[joueur.y, joueur.x] = 'J';
        grille[sortie.y, sortie.x] = 'S';

        // Placement des clés de manière aléatoire
        int nombreCles = tentative; // Nombre de clés limité à 4 après la 4ème tentative
        while (cles.Count < nombreCles)
        {
            int x = rnd.Next(1, taille - 1);
            int y = rnd.Next(1, taille - 1);
            if (grille[y, x] == ' ')
            {
                grille[y, x] = 'C';
                cles.Add((x, y)); // Stocker la position de la clé
            }
        }
    }

    static void CreerChemin(int x1, int y1, int x2, int y2)
    {
        // Algorithme simple pour créer un chemin direct entre deux points
        while (x1 != x2 || y1 != y2)
        {
            if (x1 < x2) x1++; //On bouge aux X pour arriver du dépar x1 ver l'arriver x2
            else if (x1 > x2) x1--; //On bouge aux X pour arriver du dépar x1 ver l'arriver x2
            else if (y1 < y2) y1++; //On bouge aux Y pour arriver du dépar y1 ver l'arriver y2
            else if (y1 > y2) y1--; //On bouge aux Y pour arriver du dépar y1 ver l'arriver y2

            if (grille[y1, x1] == '#') grille[y1, x1] = ' '; // Enlever les murs sur le chemin
        }
    }

    static void AfficherLabyrinthe()
    {
        
        Console.Clear(); // Effacer la console pour un nouvel affichage

        for (int i = 0; i < taille; i++)
        {

            for (int j = 0; j < taille; j++)
            {
                
                // Changer la couleur selon l'élément
                if (grille[i, j] == 'C')
                    Console.ForegroundColor = ConsoleColor.DarkYellow; // Clé en jaune foncé
                else if (grille[i, j] == 'S')
                    Console.ForegroundColor = ConsoleColor.Cyan; // Sortie en Cyan
                else if (grille[i, j] == 'J')
                    Console.ForegroundColor = ConsoleColor.Green; // Joueur en vert
                else if (grille[i, j] == '#')
                    Console.ForegroundColor = ConsoleColor.Magenta; // Joueur en Magenta
                else
                    Console.ResetColor(); // Réinitialiser la couleur
                
                Console.Write(grille[i, j] + " "); // Affichage de chaque cellule du labyrinthe
            }
           
            Console.WriteLine(); // Nouvelle ligne après chaque rangée

        }

        // Affichage du nombre de clés récupérées et à récupérer
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor= ConsoleColor.Black;
        Console.Write("\nCLÉS RÉCUPÉRÉES: ");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.WriteLine(clesRecuperees.Count + " / " + cles.Count);
        Console.BackgroundColor= ConsoleColor.Gray;
        Console.ForegroundColor= ConsoleColor.Red;
        Console.WriteLine("\nVOUS DEVEZ RÉCUPÉRER TOUTES LES CLÉS AVANT DE SORTIR !\n");
        Console.ResetColor();
    }

    static void DeplacerJoueur()
    {
        ConsoleKeyInfo touche = Console.ReadKey(); // Lire la touche pressée par le joueur
        (int newX, int newY) = joueur;

        // Déplacer le joueur selon la touche pressée
        switch (touche.Key)
        {
            case ConsoleKey.UpArrow: newY--; break; //flesh vers le haut déplacement vers le haut
            case ConsoleKey.DownArrow: newY++; break; //flesh vers le bas déplacement vers le bas
            case ConsoleKey.LeftArrow: newX--; break; //flesh vers le gauche déplacement vers le gauche
            case ConsoleKey.RightArrow: newX++; break; //flesh vers la droite déplacement vers la droite

            case ConsoleKey.W: newY--; break; //Button W déplacement vers le haut
            case ConsoleKey.S: newY++; break; //Button S déplacement vers le bas  
            case ConsoleKey.A: newX--; break; //Button A déplacement vers le gauche
            case ConsoleKey.D: newX++;break; //Button D déplacement vers la droite
        }

        // Vérifier si le déplacement est valide (ne pas traverser les murs)
        if (grille[newY, newX] != '#')
        {
            // Vérifier si le joueur récupère une clé
            if (grille[newY, newX] == 'C')
            {
                clesRecuperees.Add((newX, newY)); // Ajouter la clé à la liste des clés récupérées
            }

            

            // Mise à jour de la position du joueur dans la grille
            grille[joueur.y, joueur.x] = ' ';
            joueur = (newX, newY);
            grille[joueur.y, joueur.x] = 'J';

        }

        // Vérifier si le joueur a atteint la sortie avant de récupérer toutes les clés
        if (joueur == sortie && clesRecuperees.Count < cles.Count)
        {
            // Ne pas permettre d'atteindre la sortie tant que toutes les clés ne sont pas récupérées
            grille[sortie.x, sortie.y] = 'S';
            joueur = (1, 1); // Retourner le joueur à la position de départ
            grille[1, 1] = 'J';


        }
        
    }
    
    static void Celebration()
    {
        //Affichage d'un message avec une sonnerie à la sortie du joueur
        Console.BackgroundColor = ConsoleColor.DarkYellow;
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("*************************************************************************************");
        Console.WriteLine("* >>>  Félicitations ! Vous avez trouvé toutes les clés et atteint la sortie !  <<< *");
        Console.WriteLine("*************************************************************************************");
        Console.ResetColor();

        Playsong();

        Thread.Sleep(2000);
        Console.ResetColor();

        
    }

    static bool VerifVictoire()
    {
        // Vérifier si le joueur a récupéré toutes les clés avant d'atteindre la sortie
        return joueur == sortie && clesRecuperees.Count == cles.Count;
    }

    //Sonnerie pour la fin apres sortir
    static void Playsong()
    {
        Beep(880, 150);  // A5
        Beep(988, 150);  // B5
        Beep(1046, 150); // C6
        Beep(1175, 200); // D6

        Thread.Sleep(100);

        // Rising action
        Beep(1318, 200); // E6
        Beep(1397, 150); // F6
        Beep(1567, 300); // G6 (holds the triumph)

        Thread.Sleep(100);

        // Final hit (resolution)
        Beep(1046, 200); // C6
        Beep(784, 600);  // G5 (dramatic drop)
    }

    //fonction pour determiner la frequence du beep, la duration et la pause apres le beep
    static void Beep(int frequency, int duration)
    {
        Console.Beep(frequency, duration);
        Thread.Sleep(50); // Petite pause pour le rythme
    }

    //fonction pour vider le clavier pour assurer que la réinisialisation de la grille et le joueurs seront bien initialiser
    static void CleanKeyBoard()
    {
        while (Console.KeyAvailable) // Nettoyage du buffer clavier pour éviter les mouvements non désirés 
        {
            Console.ReadKey(true);
        }
    }

}





