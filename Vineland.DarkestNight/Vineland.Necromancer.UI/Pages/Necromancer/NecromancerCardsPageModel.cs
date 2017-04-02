using System;
using Vineland.Necromancer.Core;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Domain;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class NecromancerCardsPageModel : BaseViewModel
	{
		NecromancerService _necromancerService;

		public NecromancerCardsPageModel (NecromancerService necromancerService)
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

		public Command LocationCommand
		{
			get
			{
				return new Command(() =>
				{
					var viewModel = new ChooseLocationPopupPageModel();
					viewModel.OnLocationSelected += (location) =>
					{
						Necromancer.LocationId = location.Id;
						RaisePropertyChanged(() => LocationName);
					};
					CoreMethods.PushPopup(pageModel:viewModel);
				});
			}
		}

		public NecomancerState Necromancer {
			get{ return Application.CurrentGame.Necromancer; }
		}

		public Command ActivateCommand {
			get {
				return new Command (async (obj) => {
					Application.SaveCurrentGame ();

					//var detectionViewModel = Resolver.Resolve<NecromancerDetectionViewModel> ();
					//if (detectionViewModel.SkipPage) {
					//	var page = await CoreMethods.PushPageModel<NecromancerActivationPageModel> ();
					//	(page.BindingContext as NecromancerActivationViewModel).Initialise (detectionViewModel.SelectedResult.DetectedHero, detectionViewModel.SelectedResult.NecromancerRoll);
					//}else{
					//	await Application.Navigation.Push<NecromancerDetectionPage>(detectionViewModel);
					//}
				});
			}
		}
	}
}

