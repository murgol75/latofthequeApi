using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace lutoftheque.api.Services
{
    public class PlayerService
    {
        private readonly lutofthequeContext context;
        public PlayerService(lutofthequeContext context)
        {
            this.context = context;
        }
        public List<PlayerParticipateDto> GetPlayers()
        {
            return context.Players
                .Include(p => p.PlayerKeywords) // on join la relation Player Keyword
                    .ThenInclude(pk => pk.FkKeyword) // on join les keyword par la FkKeyword
                .Include(p => p.PlayerThemes)  // on join la relation Player Theme
                    .ThenInclude(pt => pt.FkTheme)  // on join les themes par la FkTheme

                .Select(p => new PlayerParticipateDto
                {
                    PlayerId = p.PlayerId, // player Id
                    Nickname = p.Nickname, // player nickname
                    Birthdate = p.Birthdate, // player Birthdate
                    email = p.Email,
                    // les keyword et leurs cotes
                    PlayerKeywords = p.PlayerKeywords
                        .Select(pk => new PlayerKeywordDto
                        {
                            Name = pk.FkKeyword.KeywordName,
                            Note = pk.KeywordNote
                        })
                        .ToList(),
                    // les themes et leurs cotes
                    PlayerThemes = p.PlayerThemes
                        .Select(pt => new PlayerThemeDto
                        {
                            Name = pt.FkTheme.ThemeName,
                            Note = pt.ThemeNote
                        })
                        .ToList(),
                    // Les jeux qui lui appartiennent
                    PlayerGames = p.PlayerGames
                    .Select(pg => new PlayerGameDto
                    {
                        Name = pg.FkGame.GameName,
                        Number = pg.NumberPossessed,
                        Eligible = pg.Eligible
                    })
                    .ToList(),
                    // Les events auxquels il participe
                    Events = p.FkEvents
                    .Select(e => e.EventId)
                    .ToList(),
                }).ToList();
        }

        public List<PlayerByEventDto> GetPlayersByEvent(int id)
        {
            return context.Players
                .Include(p => p.PlayerKeywords)  // on join la table d'association Players Keywords
                    .ThenInclude(pk => pk.FkKeyword)  // on la relie via la FK Keyword
                .Include(p => p.PlayerThemes)  // on join la table d'association Players Themes
                    .ThenInclude(pt => pt.FkTheme)  // on la relie via la FK Theme
                .Include(p => p.FkEvents)  // on join la Fk Event
                .Where(p => p.FkEvents.Any(e => e.EventId == id)) // on ne garde que tous les joueurs qui participent à l'event qui correspond à l'ID
                .Select(p => new PlayerByEventDto
                {
                    PlayerId = p.PlayerId, // player Id
                    Nickname = p.Nickname, // player nickname
                    Birthdate = p.Birthdate, // player Birthdate
                                             // dans l'evènement choisi

                    // les keyword et leurs cotes
                    PlayerKeywords = p.PlayerKeywords
                        .Select(pk => new PlayerKeywordDto
                        {
                            Name = pk.FkKeyword.KeywordName,
                            Note = pk.KeywordNote
                        })
                        .ToList(),
                    // les themes et leurs cotes
                    PlayerThemes = p.PlayerThemes
                        .Select(pt => new PlayerThemeDto
                        {
                            Name = pt.FkTheme.ThemeName,
                            Note = pt.ThemeNote
                        })
                        .ToList(),
                    // Les jeux qui lui appartiennent
                    PlayerGames = p.PlayerGames
                    .Select(pg => new PlayerGameDto
                    {
                        Name = pg.FkGame.GameName,
                        Number = pg.NumberPossessed,
                        Eligible = pg.Eligible
                    })
                    .ToList(),
                    // Les events auxquels il participe
                }).ToList();
        }

        public Player GetPlayerById(int id)
        {
            // D'abbord créer le modele de retour : je prends le model de base : Player
            // ce qui inclus :
            /*PlayerId
             Nickname
             Email
             Birthdate
             IsAdmin
             HashPwd
             Events (ceux auxquels il participe, il faudra faire un where)
             PlayerGames (pour ressortir tous les jeux where FkplayerId = playerId ete nbpossessed>0)
             PlayerKeywords
             PlayerTheme
             FkEvents*/
            // ensuite faire la requete return context. ...

            var player = context.Players
                .Include(p => p.PlayerKeywords)  // on join la table d'association Players Keywords
                    .ThenInclude(pk => pk.FkKeyword)  // on la relie via la FK Keyword
                .Include(p => p.PlayerThemes)  // on join la table d'association Players Themes
                    .ThenInclude(pt => pt.FkTheme)  // on la relie via la FK Theme
                .Include(p => p.FkEvents)  // on join la Fk Event
                .Where(p => p.PlayerId == id) // uniquement pour le bon playerID
                .Select(p => new Player
                {
                    PlayerId = p.PlayerId, // player Id
                    Nickname = p.Nickname, // player nickname
                    Birthdate = p.Birthdate, // player Birthdate
                    Email = p.Email, // player Email
                    HashPwd = p.HashPwd, // son mot de passe haché (à mon avis il ne le faut pas pour les détails, mais comme je vais surement reprendre ce truc pour la modification autant le laisser)
                    IsAdmin = p.IsAdmin, // pour savoir s'il est admin (à mon avis c'est utile uniquement quand c'est l'admin qui récupère ça pour pouvoir lui changer les rôles, du coup je pense pas que ce soit utile ici car quand je ferai les rôles, je ne récupèrerai que la liste des players et leurs rôles (donc nouveau modèle à faire))
                    // les keyword et leurs cotes
                    PlayerKeywords = p.PlayerKeywords
                        .Select(pk => new PlayerKeyword
                        {
                            FkKeywordId = pk.FkKeywordId, // ID du mot-clé
                            FkPlayerId = pk.FkPlayerId, // ID du joueur
                            KeywordNote = pk.KeywordNote, // Note attribuée par le joueur
                            FkKeyword = new Keyword // Créer une nouvelle instance de Keyword
                            {
                                KeywordName = pk.FkKeyword.KeywordName // Récupérer le nom du mot-clé
                            }
                        })
                        .ToList(),
                    // les themes et leurs cotes
                    PlayerThemes = p.PlayerThemes
                        .Select(pt => new PlayerTheme
                        {
                            FkThemeId = pt.FkThemeId, // ID du thème
                            FkPlayerId = pt.FkPlayerId, // ID du joueur
                            ThemeNote = pt.ThemeNote, // Note attribuée par le joueur
                            FkTheme = new Theme // Créer une nouvelle instance de Theme
                            {
                                ThemeName = pt.FkTheme.ThemeName // Récupérer le nom du thème
                            }
                        })
                        .ToList(),
                    // Les jeux qui lui appartiennent
                    PlayerGames = p.PlayerGames
                    .Select(pg => new PlayerGame
                    {
                        PlayerGameId = pg.PlayerGameId, // ID du jeu du joueur
                        FkPlayerId = pg.FkPlayerId, // ID du joueur
                        FkGameId = pg.FkGameId, // ID du jeu
                        NumberPossessed = pg.NumberPossessed, // Nombre de jeux possédés par le joueur
                        Eligible = pg.Eligible, // Éligibilité du jeu
                        FkGame = new Game // Créer une nouvelle instance de Game
                        {
                            GameName = pg.FkGame.GameName // Récupérer le nom du jeu
                                                          // Vous pouvez ajouter d'autres propriétés du jeu si nécessaire
                        }
                    })
                    .ToList(),
                    // Les events auxquels il participe
                    Events = p.FkEvents
                    .Select(fe => new Event
                    {
                        EventId = fe.EventId,
                        StartTime = fe.StartTime,
                        EndTime = fe.EndTime,
                        RegistrationClosingDate = fe.RegistrationClosingDate,
                        EventName = fe.EventName,
                        FkOrganizerId = fe.FkOrganizerId
                    })
                    .ToList(),

                }).FirstOrDefault();



            return player;



        }

    }
}
