using lutoftheque.api.Dto;
using lutoftheque.api.Exceptions;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace lutoftheque.api.Services
{
    public class GameService
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public GameService(lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
        }


        //public int GameId { get; set; }
        //public string? GameName { get; set; }
        //public int PlayersMin { get; set; }
        //public int PlayersMax { get; set; }
        //public int AverageDuration { get; set; }
        //public int AgeMin { get; set; }
        //public string? Picture { get; set; }
        //public string? FkTheme { get; set; }
        //public List<string>? FkKeywords { get; set; }
        //public List<string>? FkSecondaryThemes { get; set; }
        public List<GameDto> GetGames()
        {
            //GameDto gameDto = ;


            return context.Games
                .Include(g => g.FkTheme)
                .Include(g => g.FkKeywords)
                .Include(g => g.FkSecondaryThemes)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    PlayersMin = g.PlayersMin,
                    PlayersMax = g.PlayersMax,
                    AverageDuration = g.AverageDuration,
                    AgeMin = g.AgeMin,
                    Picture = g.Picture,
                    FkTheme = g.FkTheme.ThemeName,
                    FkKeywords = g.FkKeywords.Select(k => k.KeywordName).ToList(),
                    FkSecondaryThemes = g.FkSecondaryThemes.Select(st => st.ThemeName).ToList()
                })
                .OrderBy(g => g.GameName)
                .ToList();
        }

        public GameFullDto? GetGameById(int id)
        {
            Game? game = context.Games  // recupère le Jeu
                .Include(g => g.FkTheme)  // en incluant le theme
                .Include(g => g.FkKeywords) // et les keywords
                .Include(g => g.FkSecondaryThemes) // et les secondary themes
                .FirstOrDefault(g => g.GameId == id); // le premier qu'il trouve dont l'id == l'id en parametre

            if (game == null)
            {
                return null;
            }

            GameFullDto gameFullDto = new GameFullDto
            {
                GameId = game.GameId,
                GameName = game.GameName,
                PlayersMin = game.PlayersMin,
                PlayersMax = game.PlayersMax,
                AverageDuration = game.AverageDuration,
                AgeMin = game.AgeMin,
                Picture = game.Picture,
                GameDescription = game.GameDescription,
                Video = game.Video,
                FkThemeId = game.FkThemeId,
                IsExtension = game.IsExtension,
                FkTheme = game.FkTheme != null ? game.FkTheme.ThemeName : null, // Vérifie si FkTheme est null
                FkKeywords = game.FkKeywords != null ? game.FkKeywords.Select(k => k.KeywordName).ToList() : new List<string>(), // Vérifie si FkKeywords est null
                FkSecondaryThemes = game.FkSecondaryThemes != null ? game.FkSecondaryThemes.Select(k => k.ThemeName).ToList() : new List<string>(), // Vérifie si FkSecondaryThemes est null
            };

            return gameFullDto;
        }

        //MemberAccessException sert pour updater le jeu
        public Game GetGameEntityById(int gameId)
        {
            return context.Games
                .Include(g => g.FkKeywords)  // Inclure les mots-clés
                .FirstOrDefault(g => g.GameId == gameId);  // Récupérer l'entité complète
        }


        public List<GameFullDto> GetGamesForActualEvent(int eventId)
        {
            return context.Games
                .Include(g => g.FkTheme)
                .Include(g => g.FkKeywords)
                .Include(g => g.FkSecondaryThemes)
                .Select(g => new GameFullDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    PlayersMin = g.PlayersMin,
                    PlayersMax = g.PlayersMax,
                    AverageDuration = g.AverageDuration,
                    AgeMin = g.AgeMin,
                    FkTheme = g.FkTheme.ThemeName,
                    FkKeywords = g.FkKeywords.Select(k => k.KeywordName).ToList(),
                    FkSecondaryThemes = g.FkSecondaryThemes.Select(st => st.ThemeName).ToList()
                }).ToList();
        }

        public List<GameLightDto> GetGamesForEvent()
        {
            return context.Games
                .Select(g => new GameLightDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    Picture = g.Picture
                }).ToList();
        }

        public void CreateGame(string gameName, int playersMin, int playersMax, int averageDuration, int ageMin, string picture, string gameDescription, string video, int? fkThemeId, bool? isExtension, List<int> fkKeywordsId)
        {
            Game newGame = new Game
            {
                GameName = gameName,
                PlayersMin = playersMin,
                PlayersMax = playersMax,
                AverageDuration = averageDuration,
                AgeMin = ageMin,
                Picture = picture,
                GameDescription = gameDescription,
                Video = video,
                FkThemeId = fkThemeId,
                IsExtension = isExtension,
            };

            #region Verification de l'existance d'un jeu avec le même nom

            var existingGame = context.Games.FirstOrDefault(p => p.GameName == gameName);
            if (existingGame != null)
            {
                throw new DuplicateNicknameException("Ce Jeu existe déjà");
            }

            #endregion

            #region Ajout des mots-clés

            if (fkKeywordsId != null && fkKeywordsId.Any())
            {
                var keywords = context.Keywords.Where(k => fkKeywordsId.Contains(k.KeywordId)).ToList();
                newGame.FkKeywords = keywords;
            }

            #endregion

            context.Games.Add(newGame);
            context.SaveChanges();  // à faire pour enregistrer l'entrée
        }


        public List<Theme> GetThemes()
        {
            return context.Themes
                .Select(t => new Theme
                {
                    ThemeId = t.ThemeId,
                    ThemeName = t.ThemeName,
                    ThemeDescription = t.ThemeDescription
                })
                .ToList();
        }

        public List<Keyword> GetKeywords()
        {
            return context.Keywords
                            .Select(k => new Keyword
                            {
                                KeywordId = k.KeywordId,
                                KeywordName = k.KeywordName,
                                KeywordDescription = k.KeywordDescription
                            })
                            .ToList();
        }


        public bool DeleteGame(int id)
        {
            var gameToDelete = context.Games
                .Include(g => g.PlayerGames)        // Inclure les PlayerGames associés
                .Include(g => g.FkKeywords)         // Inclure les Keywords associés
                .Include(g => g.FkSecondaryThemes)  // Inclure les SecondaryThemes associés
                .FirstOrDefault(g => g.GameId == id);

            if (gameToDelete == null)
            {
                return false; // Jeu non trouvé
            }

            // Supprimer les enregistrements associés dans PlayerGames
            if (gameToDelete.PlayerGames != null && gameToDelete.PlayerGames.Any())
            {
                context.PlayerGames.RemoveRange(gameToDelete.PlayerGames);
            }

            // Supprimer les mots-clés associés
            if (gameToDelete.FkKeywords != null && gameToDelete.FkKeywords.Any())
            {
                gameToDelete.FkKeywords.Clear(); // Dissocier les mots-clés
            }

            // Supprimer les SecondaryThemes associés
            if (gameToDelete.FkSecondaryThemes != null && gameToDelete.FkSecondaryThemes.Any())
            {
                gameToDelete.FkSecondaryThemes.Clear(); // Dissocier les secondary themes
            }

            // Supprimer le jeu principal
            context.Games.Remove(gameToDelete);

            // Sauvegarder les changements dans la base de données
            context.SaveChanges();

            return true; // Suppression réussie
        }


        public void UpdateGame(int gameId, string gameName, int playersMin, int playersMax, int averageDuration, int ageMin, string picture, string gameDescription, string video, int? fkThemeId, bool? isExtension, List<int> fkKeywordsId)
        {
            var gameFromDb = context.Games
                .Include(g => g.FkKeywords)
                .FirstOrDefault(g => g.GameId == gameId);

            //if (gameFromDb == null)
            //{
            //    throw new NotFoundException("Game not found");
            //}

            // Mise à jour des propriétés du jeu
            gameFromDb.GameName = gameName;
            gameFromDb.PlayersMin = playersMin;
            gameFromDb.PlayersMax = playersMax;
            gameFromDb.AverageDuration = averageDuration;
            gameFromDb.AgeMin = ageMin;
            gameFromDb.Picture = picture;
            gameFromDb.GameDescription = gameDescription;
            gameFromDb.Video = video;
            gameFromDb.FkThemeId = fkThemeId;
            gameFromDb.IsExtension = isExtension;

            // Gestion des mots-clés
            if (fkKeywordsId != null && fkKeywordsId.Any())
            {
                // Effacer les mots-clés actuels et les remplacer par les nouveaux
                gameFromDb.FkKeywords.Clear();
                var keywords = context.Keywords.Where(k => fkKeywordsId.Contains(k.KeywordId)).ToList();
                foreach (var keyword in keywords)
                {
                    gameFromDb.FkKeywords.Add(keyword);
                }
            }

            // Sauvegarder les modifications dans la base de données
            context.SaveChanges();
        }







    }
}