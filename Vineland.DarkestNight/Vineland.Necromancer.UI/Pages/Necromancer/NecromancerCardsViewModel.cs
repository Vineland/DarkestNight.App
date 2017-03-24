using System;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using XLabs.Ioc;
using Vineland.Necromancer.Domain;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class NecromancerCardsViewModel : BaseViewModel
	{
		NecromancerService _necromancerService;

		public NecromancerCardsViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;

			_necromancerService.AdjustDarkness(Application.CurrentGame);
		}

		public int Darkness {
			get { return Application.CurrentGame.Darkness; }
			set{ Application.CurrentGame.Darkness = value; }
		}

		public string LocationName
		{
			get
			{
				return Application.CurrentGame.Locations.Single(l => l.Id == Necromancer.LocationId).Name;
			}
		}

		public RelayCommand LocationCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					var viewModel = new ChooseLocationPopupViewModel();
					viewModel.OnLocationSelected += (location) =>
					{
						Necromancer.LocationId = location.Id;
						RaisePropertyChanged(() => LocationName);
					};
					Application.Navigation.PushPopup<ChooseLocationPopupPage>(viewModel);
				});
			}
		}

		public NecomancerState Necromancer {
			get{ return Application.CurrentGame.Necromancer; }
		}

		public RelayCommand ActivateCommand {
			get {
				return new RelayCommand (async () => {
					Application.SaveCurrentGame ();

					var detectionViewModel = Resolver.Resolve<NecromancerDetectionViewModel> ();
					if (detectionViewModel.SkipPage) {
						var page = await Application.Navigation.Push<NecromancerActivationPage> ();
						(page.BindingContext as NecromancerActivationViewModel).Initialise (detectionViewModel.SelectedResult.DetectedHero, detectionViewModel.SelectedResult.NecromancerRoll);
					}else{
						await Application.Navigation.Push<NecromancerDetectionPage>(detectionViewModel);
					}
				});
			}
		}
	}
}

