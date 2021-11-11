/**
* Laboratoire intégration des méthodes
* 
* Simulation d'un combat entre 2 personnages, Sergent Ebellius et Pustus le Vile
* 4 stats par personnage : pv, armure, agilite, dommage
* Ordre d'attaque : le plus haut en combinant agilite et un nombre aléatoire de 1 à 10
* Calcul des dommages (pourcentage aléatoire)
*  5- : 0
*  6 - 60 : dommage x 1/2
*  61 - 90 : dommage 
*  91+ : dommage x 1.5
* 
* L'armure prend le dommage d'abord, puis les pv
* 
* Création : 21-11-11
* Par : Frédérik Taleb
*
* Modification: 21-11-11
* Par : Frédérik Taleb
*/

using System;

namespace Labo_Tour_de_Zork_Combat_01
{
    class Program
    {
        static void Main(string[] args)
        {
            // Quand on intègre les classes, on prend les stats et elles deviennent des attributs
            // de la classe!

            // Acteur pour Sergent Ebellius
            Acteur sergentEbellius = new Acteur("Sergent Ebellius", 20, 20, 12, 7);
            // Acteur pour Pustus le Vile
            Acteur pustusLeVile = new Acteur("Pustus le Vile", 30, 10, 8, 11);
            // Variables pour l'initiative
            int initiativeSergent = 0;
            int initiativePustus = 0;

            // Le combat sera fini quand un des deux personnages arrive à 0 pv
            while (sergentEbellius.Hp > 0 && pustusLeVile.Hp > 0)
            {
                // Celui qui a l'initiative la plus haute frappe en premier
                // Si l'autre n'est pas mort, il réplique avec ses dommages
                // Quand l'initiative est égale, les 2 dommages sont assignés sans exception
                initiativeSergent = GenererInitiative(sergentEbellius.Agilite);
                initiativePustus = GenererInitiative(pustusLeVile.Agilite);
                if (initiativeSergent > initiativePustus)
                {
                    // Il faut vérifier l'armure avant d'enlever des points de vie
                    // En enlevant toujours les dégâts à l'armure on finit par avoir une nombre négatif
                    //
                    // Quand on a un nombre négatif, on enlève ce nombre aux points de vie et on remet l'armure à 0 
                    // pour ne pas fausser les calculs subséquents
                    //
                    // Il serait possible de gérer l'assignation des dommages avec une méthode, mais je vous épargne
                    // le mal de tête
                    pustusLeVile.Armure -= GenererDommages(sergentEbellius.Dommage);
                    if (pustusLeVile.Armure < 0)
                    {
                        // la variable d'armure du personnage contient un nombre négatif
                        // il représente le nombre de dommages ayant dépassé l'armure
                        // donc on additionne le nombre négatif aux points de vie pour avoir un 
                        // résultat cohérent
                        // ex: pustusLeVile.Armure -3 & pustusLeVile.Hp 30 
                        //     en faisant pustusLeVile.Hp += pustusLeVile.Armure on a 30 + -3
                        pustusLeVile.Hp += pustusLeVile.Armure;
                        // Il ne faut pas oublier de remettre l'armure à 0, sinon on a encore les dommages du tour précédent
                        pustusLeVile.Armure = 0;
                    }
                    // On assigne les dégâts au premier personnage seulement si le 2ème est encore vivant
                    if (pustusLeVile.Hp > 0)
                    {
                        sergentEbellius.Armure -= GenererDommages(pustusLeVile.Dommage);
                        if (sergentEbellius.Armure < 0)
                        {
                            sergentEbellius.Hp += sergentEbellius.Armure;
                            sergentEbellius.Armure = 0;
                        }
                    }
                }
                else if (initiativePustus > initiativeSergent)
                {
                    // On répète le processus mais dans un ordre différent
                    sergentEbellius.Armure -= GenererDommages(pustusLeVile.Dommage);
                    if (sergentEbellius.Armure < 0)
                    {
                        sergentEbellius.Hp += sergentEbellius.Armure;
                        sergentEbellius.Armure = 0;
                    }
                    if (sergentEbellius.Hp > 0)
                    {
                        pustusLeVile.Armure -= GenererDommages(sergentEbellius.Dommage);
                        if (pustusLeVile.Armure < 0)
                        {
                            pustusLeVile.Hp += pustusLeVile.Armure;
                            pustusLeVile.Armure = 0;
                        }
                    }
                }
                else
                {
                    // Si les 2 personnages se frappent simultanément c'est encore le même processus mais 
                    // les dommages sont assignés même si les points de vie du premier tombent sous 0
                    sergentEbellius.Armure -= GenererDommages(pustusLeVile.Dommage);
                    if (sergentEbellius.Armure < 0)
                    {
                        sergentEbellius.Hp += sergentEbellius.Armure;
                        sergentEbellius.Armure = 0;
                    }
                    pustusLeVile.Armure -= GenererDommages(sergentEbellius.Dommage);
                    if (pustusLeVile.Armure < 0)
                    {
                        pustusLeVile.Hp += pustusLeVile.Armure;
                        pustusLeVile.Armure = 0;
                    }
                }

                // Afficher les stats défensive de chaque personnage à la fin du tour
                Console.WriteLine($"{pustusLeVile.Nom} armure: {pustusLeVile.Armure}  pv: {pustusLeVile.Hp}");
                Console.WriteLine($"{sergentEbellius.Nom} armure: {sergentEbellius.Armure} pv: {sergentEbellius.Hp}");
                Console.WriteLine("Appuyer sur une touche pour continuer");
                Console.ReadKey();
                Console.Clear();
            }

            // Fin du combat, on affiche le résultat
            Console.Clear();
            if (sergentEbellius.Hp <= 0 && pustusLeVile.Hp > 0)
            {
                Console.WriteLine($"{sergentEbellius.Nom} est mort, {pustusLeVile.Nom} a gagné");
            }
            else if (pustusLeVile.Hp <= 0 && sergentEbellius.Hp > 0)
            {
                Console.WriteLine($"{pustusLeVile.Nom} est mort, {sergentEbellius.Nom} a gagné");
            }
            else
            {
                Console.WriteLine($"Les deux combatants sont morts!");
            }
            Console.ReadKey();
        }

        /**
        * Génère une initiative aléatoire en utilisant l'agilité du personnage
        * 
        * @param agilite un nombre entier représentant l'agilité de base du personnage
        * @return un nombre aléatoire modifié par l'agilité
        */
        static int GenererInitiative(int agilite)
        {
            Random rng = new Random();
            return agilite + rng.Next(1,11);
        }

        /**
        * Selon un pourcentage, calcule les dommages causés par un personnage en se basant sur la statistique dommage du personnage
        * 
        * Comme il n'est pas possible de multiplier un int par un double,
        * on utilise une multiplication par 3 puis division par deux pour les coups critiques
        *
        * @param dommage les dommages de base du personnage
        * @return les dommages finaux causés par le personnage
        */
        static int GenererDommages(int dommage)
        {
            Random rng = new Random();
            int jetTouche = rng.Next(1,101);
            int dommageFinal = 0;
            if(jetTouche > 90)
            {
                dommageFinal = dommage * 3 / 2;
            }
            else if (jetTouche > 60)
            {
                dommageFinal = dommage;
            }
            else if(jetTouche > 5)
            {
                dommageFinal = dommage / 2;
            }

            return dommageFinal;
        }
    }
}
