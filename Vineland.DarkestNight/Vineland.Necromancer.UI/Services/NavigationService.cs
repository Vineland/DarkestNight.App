﻿using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class NavigationService
	{
		INavigation _navigation;
		PageService _pageService;

		public NavigationService (PageService pageService)
		{
			_pageService = pageService;

			//_navigation = new NavigationPage (_pageService.CreatePage<HomePage> ());
		}

		public void SetNavigation (INavigation navigation)
		{
			_navigation = navigation;

		}

		public async void Pop ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			
			await _navigation.PopAsync ();
		}

		public async void PopToRoot ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			await _navigation.PopToRootAsync ();
		}

		public async void PopTo<T> () where T: Page
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			//TODO: needs to be more robust
			//remove all pages between the current page and target page
			for (int i = _navigation.NavigationStack.Count - 2; i >= 0; i--) {
				if (_navigation.NavigationStack [i].GetType () == typeof(T))
					break;
				_navigation.RemovePage (_navigation.NavigationStack [i]);
			}
			//pop the current page
			await _navigation.PopAsync ();
		}

		public async void Push<T> (bool clearBackStack = false) where T : Page
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			try {
					var page = _pageService.CreatePage<T>();
					await _navigation.PushAsync (page as Page);
					if(clearBackStack){

						//remove all pages between the current page and target page
						for (int i = 1; i < _navigation.NavigationStack.Count - 1; i++) {
							_navigation.RemovePage (_navigation.NavigationStack [i]);
						}
					}
			} catch (Exception ex) {
				throw ex;
			}
		}
	}
}

