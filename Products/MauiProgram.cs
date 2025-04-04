using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using Microsoft.Maui.Handlers;

namespace Products;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionToolkit()
            .ConfigureMauiHandlers(handlers =>
            {
                // Remove underline from Entry on Android
                handlers.AddHandler<Entry, EntryHandler>();
                EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
                {
#if ANDROID
                    handler.PlatformView.Background = null; // Removes the underline
#endif
                });
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                fonts.AddFont("IrishGrover-Regular.ttf", "IrishGrover");
                fonts.AddFont("IslandMoments-Regular.ttf", "IslandMoments");
                fonts.AddFont("InstrumentSerif-Regular.ttf", "InstrumentSerif");
                fonts.AddFont("Font-Awesome-6-Free-Regular-400.ttf", "FontAwesome");
            });

#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        builder.Services.AddSingleton<ProjectRepository>();
        builder.Services.AddSingleton<TaskRepository>();
        builder.Services.AddSingleton<CategoryRepository>();
        builder.Services.AddSingleton<TagRepository>();
        builder.Services.AddSingleton<SeedDataService>();
        builder.Services.AddSingleton<ModalErrorHandler>();
        builder.Services.AddSingleton<MainPageModel>();
        builder.Services.AddSingleton<ProjectListPageModel>();
        builder.Services.AddSingleton<ManageMetaPageModel>();

        builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
        builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");

        return builder.Build();
    }
}
