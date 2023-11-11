using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Al2Me6.PowerToys.Run.Plugin.Qalculate
{
    public class Main : IPlugin, IPluginI18n, ISettingProvider, IDisposable
    {
        public static string PluginID => "E9DDBD21B1AE491AAF4C51B389C737AD";

        private bool _disposed;

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        public string Name => Properties.Resources.plugin_name;

        public string Description => Properties.Resources.plugin_description;

        public string GetTranslatedPluginTitle() => Name;

        public string GetTranslatedPluginDescription() => Description;

        private const string CONFIG_KEY_PATH = "QalculatePath";

        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>()
        {
            new()
            {
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                Key = CONFIG_KEY_PATH,
                DisplayLabel = Properties.Resources.qalc_path,
                DisplayDescription = Properties.Resources.qalc_path_desc,
                TextValue = "",
            }
        };

        private readonly QalculateHelper helper = new();

        public void Init(PluginInitContext? context)
        {
            Context = context ?? throw new ArgumentNullException(paramName: nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        public List<Result> Query(Query? query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(paramName: nameof(query));
            }

            string? search = query.Search?.Trim();
            bool isGlobalQuery = string.IsNullOrEmpty(query.ActionKeyword);

            if (!helper.QalculateFound || string.IsNullOrEmpty(search))
            {
                return new();
            }

            if (isGlobalQuery && !QalculateHelper.ShouldEvaluateGlobally(search))
            {
                return new();
            }

            try
            {
                var result = helper.Evaluate(search);

                if (result == null)
                {
                    return new();
                }

                return new()
                {
                    new Result
                    {
                        Title = result,
                        IcoPath = IconPath,
                        Score = 300,
                        SubTitle = Properties.Resources.copy_to_clipboard,
                        Action = ctx =>
                        {
                            var ret = false;

                            var thread = new Thread(() =>
                            {
                                try
                                {
                                    Clipboard.SetText(result);
                                    ret = true;
                                }
                                catch (ExternalException)
                                {
                                    MessageBox.Show(Properties.Resources.copy_failed);
                                }
                            });

                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start();
                            thread.Join();

                            return ret;
                        },
                    },
                };
            }
            catch (Exception ex)
            {
                return MakeErrorResult(isGlobalQuery, query.RawQuery, ex);
            }
        }

        private List<Result> MakeErrorResult(bool isGlobal, string rawQuery, Exception ex)
        {
            Log.Error($"Encountered error while evaluating query `{rawQuery}`: `{ex}`.", GetType());

            if (isGlobal)
            {
                return new();
            }

            return new()
            {
                new Result
                {
                    Title = Properties.Resources.failure_title,
                    SubTitle = ex.Message,
                    IcoPath = IconPath,
                    Score = 300,
                },
            };
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconPath = "Images/calculator.light.png";
            }
            else
            {
                IconPath = "Images/calculator.dark.png";
            }
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            if (settings.AdditionalOptions == null)
            {
                return;
            }

            var path = settings.AdditionalOptions.FirstOrDefault(o => o.Key == CONFIG_KEY_PATH)?.TextValue;
            helper.OverridePath(path);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Context != null && Context.API != null)
                    {
                        Context.API.ThemeChanged -= OnThemeChanged;
                    }

                    _disposed = true;
                }
            }
        }
    }
}
