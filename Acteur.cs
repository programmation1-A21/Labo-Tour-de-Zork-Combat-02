namespace Labo_Tour_de_Zork_Combat_01
{
    class Acteur
    {
        public string Nom { get; set; }
        public int Hp { get; set; }
        public int Armure { get; set; }
        public int Agilite { get; set; }
        public int Dommage { get; set; }

        public Acteur(string nom, int hp, int armure, int agilite, int dommage)
        {
            this.Nom = nom;
            this.Hp = hp;
            this.Armure = armure;
            this.Agilite = agilite;
            this.Dommage = dommage;
        }

    }
}