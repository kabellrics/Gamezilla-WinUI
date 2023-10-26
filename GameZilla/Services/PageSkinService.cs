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

        public IEnumerable<String> GetDisplaysForHome() { return _displayhome; }
        public IEnumerable<String> GetDisplaysForSystems() { return _displaysystems; }
        public IEnumerable<String> GetDisplaysForGames() { return _displaygames; }
        public IEnumerable<String> GetDisplaysForGameDetail() { return _displaydetailgame; }

        public async Task<string> GetCurrentDisplayHome()
        {
            var homeskin = await _options.ReadSettingAsync<string>("homeskin");
            if (string.IsNullOrEmpty(homeskin))
                return "Basic";
            return homeskin;
        }
        public async void SetCurrentDisplayHome(string value)
        {
            await _options.SaveSettingAsync<string>("homeskin", value);
        }
        public async Task<string> GetCurrentDisplaySystems()
        {
            var homeskin = await _options.ReadSettingAsync<string>("sysskin");
            if (string.IsNullOrEmpty(homeskin))
                return "Basic";
            return homeskin;
        }
        public async void SetCurrentDisplaySystems(string value)
        {
            await _options.SaveSettingAsync<string>("sysskin", value);
        }
        public async Task<string> GetCurrentDisplayGames()
        {
            var homeskin = await _options.ReadSettingAsync<string>("gameskin");
            if (string.IsNullOrEmpty(homeskin))
                return "Basic";
            return homeskin;
        }
        public async void SetCurrentDisplayGames(string value)
        {
            await _options.SaveSettingAsync<string>("gameskin", value);
        }
        public async Task<string> GetCurrentDisplayGameDetail()
        {
            var homeskin = await _options.ReadSettingAsync<string>("gamedetailskin");
            if (string.IsNullOrEmpty(homeskin))
                return "Basic";
            return homeskin;
        }
        public async void SetCurrentDisplayGameDetail(string value)
        {
            await _options.SaveSettingAsync<string>("gamedetailskin", value);
        }
    }
}
