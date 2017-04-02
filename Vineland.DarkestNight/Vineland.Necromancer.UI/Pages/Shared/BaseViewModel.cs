using System;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using FreshMvvm;
using System.Linq.Expressions;

namespace Vineland.Necromancer.UI
{
	public abstract class BaseViewModel :FreshBasePageModel
	{
		public BaseViewModel ()
		{
		}

		public NecromancerApp Application
		{
			get { return (Xamarin.Forms.Application.Current as NecromancerApp); }
		}

		bool _isLoading;
		public bool IsLoading{
			get{ return _isLoading; }
			set{
				if (_isLoading != value) {
					_isLoading = value;
					RaisePropertyChanged(() => IsLoading);
				}
			}
		}

		protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> selectorExpression)
		{
			if (selectorExpression == null)
				throw new ArgumentNullException("selectorExpression");
			
			MemberExpression body = selectorExpression.Body as MemberExpression;
			if (body == null)
				throw new ArgumentException("The body must be a member expression");
			
			RaisePropertyChanged(body.Member.Name);
		}
	}
}

