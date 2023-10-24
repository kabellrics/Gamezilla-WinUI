using GameZilla.Contracts.Services;
using GameZilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Services
{
    public class PageSkinService : IPageSkinService
    {
        private readonly ILocalSettingsService _options;
        private IEnumerable<String> _displayhome;
        private IEnumerable<String> _displaysystems;
        private IEnumerable<String> _displaygames;
        private IEnumerable<String> _displaydetailgame;
        public PageSkinService(ILocalSettingsService options)
        {
            _options = options;
            _displayhome = new List<String>() { "Basic", "Hero" };
            _displaysystems = new List<String>() { "Basic", "Hero" };
            _displaygames = new List<String>() { "Basic", "Hero" };
            _displaydetailgame = new List<String>() { "Basic", "Hero" };
        }

        public IEnumerable<String> GetDisplayForHome() { return _displayhome; }
        public IEnumerable<String> GetDisplayForSystems() { return _displaysystems; }
        public IEnumerable<String> GetDisplayForGames() { return _displaygames; }
        public IEnumerable<String> GetDisplayForGameDetail() { return _displaydetailgame; }
    }
}
