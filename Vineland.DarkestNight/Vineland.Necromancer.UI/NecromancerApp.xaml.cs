using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using Vineland.Necromancer.Core;
using System.Threading.Tasks;
using Plugin.Toasts;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Vineland.Necromancer.UI
{
	public partial class NecromancerApp : Application
	{
		public NavigationService Navigation { get; private set; }
		public IToastNotificator ToastService { get; private set; }
		GameStateService _gameStateService;

		public NecromancerApp ()
		{
			InitializeComponent ();

			_gameStateService = Resolver.Resolve<GameStateService> ();
			
			MainPage = new CustomNavigationPage (Resolver.Resolve<PageService> ().CreatePage<HomePage> ());

			Navigation = Resolver.Resolve<NavigationService> ();
			ToastService = Resolver.Resolve<IToastNotificator>();
				Navigation.SetNavigation (MainPage.Navigation);
		}

		public GameState CurrentGame { get; set;}

		public void SaveCurrentGame ()
		{
			Task.Run (() => {
				_gameStateService.SaveCurrentGame(AppConstants.SaveFilePath, CurrentGame);
			});
		}

		protected override void OnStart ()
		{
			CurrentGame = _gameStateService.LoadGame(AppConstants.SaveFilePath);
		}

		protected override void OnSleep ()
		{
		}

		protected override void OnResume ()
		{
		}
	}
}

