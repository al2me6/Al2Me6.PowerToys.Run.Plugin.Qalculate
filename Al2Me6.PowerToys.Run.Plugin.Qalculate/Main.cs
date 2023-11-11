using System.Runtime.InteropServices;
using System.Windows;
using ManagedCommon;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Al2Me6.PowerToys.Run.Plugin.Qalculate
{
    public class Main : IPlugin, IPluginI18n, IDisposable
    {
        public static string PluginID => "E9DDBD21B1AE491AAF4C51B389C737AD";

        private bool _disposed;

        private PluginInitContext? Context { get; set; }

        private string? IconPath { get; set; }

        public string Name => Properties.Resources.plugin_name;

        public string Description => Properties.Resources.plugin_description;

        public string GetTranslatedPluginTitle() => Name;

        public string GetTranslatedPluginDescription() => Description;

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

            if (!QalculateHelper.QalculateFound || string.IsNullOrEmpty(search))
            {
                return new();
            }

            if (isGlobalQuery && !QalculateHelper.ShouldEvaluateGlobally(search))
            {
                return new();
            }

            try
            {
                var result = QalculateHelper.Evaluate(search);

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
