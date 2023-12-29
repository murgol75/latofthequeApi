using System.ComponentModel.DataAnnotations;

namespace lutoftheque.api.Dto
{
    // keyword tel qu'il est attendu en sortie de requete
    public class KeywordDto
    {
        public int KeywordId { get; set; }

        public string? KeywordName { get; set; }

        public string? KeywordDescription { get; set; }
    }

    //keyword tel qu'il est attendu pour ajouter/modifier des données
    public class KeywordDataDto
    {
        public int KeywordId { get; set; }

        [Required]
        public string? KeywordName { get; set; }

        public string? KeywordDescription { get; set; }
    }
    // Keyword renvoyé dans un jeu
    public class KeywordLightDto
    {
        public int KeywordId { get; set; }
        public string? KeywordName { get; set; }

    }
}
