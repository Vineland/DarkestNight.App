using System;

namespace Vineland.Necromancer.Domain
{
	[Flags]
	public enum Expansion
	{
		BaseGame = 0,
		WithAnInnerLight = 1,
		OnShiftingWinds =2,
		FromTheAbyss = 4,
		InTalesOfOld = 8,
		NymphPromo = 16,
		EnchanterPromo = 32,
		MercenaryPromo = 64,
		TinkerPromo = 128
	}
}

