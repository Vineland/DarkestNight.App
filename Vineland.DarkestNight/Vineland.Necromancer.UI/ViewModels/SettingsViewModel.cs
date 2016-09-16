using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class SettingsViewModel : BaseViewModel
	{
		public SettingsViewModel (Settings settings)
		{
			Settings = settings;
		}

		public Settings Settings {get; private set;}

		public bool WithAnInnerLight
		{
			get { return Settings.Expansions.HasFlag(Expansion.WithAnInnerLight); }
			set
			{
				if (value)
					AddExpansion(Expansion.WithAnInnerLight);
				else
					RemoveExpansion(Expansion.WithAnInnerLight);
				
				Settings.UseQuests = false;
			}
		}

		public bool OnShiftingWinds
		{
			get { return Settings.Expansions.HasFlag(Expansion.OnShiftingWinds); }
			set
			{
				if (value)
					AddExpansion(Expansion.OnShiftingWinds);
				else
					RemoveExpansion(Expansion.OnShiftingWinds);

			}
		}

		public bool FromTheAbyss
		{
			get { return Settings.Expansions.HasFlag(Expansion.FromTheAbyss); }
			set
			{
				if (value)
					AddExpansion(Expansion.FromTheAbyss);
				else
					RemoveExpansion(Expansion.FromTheAbyss);
				
				Settings.DarknessCardsMode = DarknessCardsMode.None;
			}
		}

		public bool InTalesOfOld
		{
			get { return Settings.Expansions.HasFlag(Expansion.InTalesOfOld); }
			set
			{
				if (value)
					AddExpansion(Expansion.InTalesOfOld);
				else
					RemoveExpansion(Expansion.InTalesOfOld); }
		}

		public bool NymphPromo
		{
			get { return Settings.Expansions.HasFlag(Expansion.NymphPromo); }
			set
			{
				if (value)
					AddExpansion(Expansion.NymphPromo);
				else
					RemoveExpansion(Expansion.NymphPromo);}
		}

		public bool EnchanterPromo
		{
			get { return Settings.Expansions.HasFlag(Expansion.EnchanterPromo); }
			set
			{
				if (value)
					AddExpansion(Expansion.EnchanterPromo);
				else
					RemoveExpansion(Expansion.EnchanterPromo); }
		}

		public bool MercenaryPromo
		{
			get { return Settings.Expansions.HasFlag(Expansion.MercenaryPromo); }
			set
			{
				if (value)
					AddExpansion(Expansion.MercenaryPromo);
				else
					RemoveExpansion(Expansion.MercenaryPromo); 
			}
		}

		public bool TinkerPromo
		{
			get { return Settings.Expansions.HasFlag(Expansion.TinkerPromo); }
			set
			{
				if (value)
					AddExpansion(Expansion.TinkerPromo);
				else
					RemoveExpansion(Expansion.TinkerPromo);
			}
		}

		private void RemoveExpansion(Expansion value)
		{
			Settings.Expansions &= ~value;
		}

		private void AddExpansion(Expansion value)
		{
			Settings.Expansions |= value;
		}
	}
}

