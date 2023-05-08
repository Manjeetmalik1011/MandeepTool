using Jci.RetailSurveyTool.TechnicianApp.Annotations;
using Jci.RetailSurveyTool.TechnicianApp.Data;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public virtual void OnAppearing()
        {
        }





        public LocalAppDatabase LocalAppDatabase => DependencyService.Resolve<LocalAppDatabase>();
        public RestService RestService => DependencyService.Resolve<RestService>();
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();






        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            //if (EqualityComparer<T>.Default.Equals(backingStore, value))
            //    return false;

            //backingStore = value;
            //onChanged?.Invoke();
            //OnPropertyChanged(propertyName);
            return true;
        }
        /*protected async Task LoadTable<T>(ObservableCollection<T> collection, Func<IEnumerable<T>> source)
        {
            IsBusy = true;
            
            try
            {
                await Task.Run(() =>
                {
                    collection.Clear();
                    //var items = await LocalAppDatabase.GetAuditTypeAsync();
                    foreach (T item in source.Invoke())
                    {
                        collection.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }*/
        protected async Task LoadTable<T>(ObservableCollection<T> collection, Func<List<T>> source)
        {
            IsBusy = true;

            try
            {
                await Task.Run(() =>
                {
                    collection.Clear();
                    //var items = await LocalAppDatabase.GetAuditTypeAsync();
                    foreach (T item in source.Invoke())
                    {
                        collection.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        protected async Task LoadTableAsync<T>(ObservableCollection<T> collection, Func<Task<IEnumerable<T>>> source)
        {
            IsBusy = true;

            try
            {
                collection.Clear();
                //var items = await LocalAppDatabase.GetAuditTypeAsync();
                foreach (T item in await source.Invoke())
                {
                    collection.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        protected async Task LoadTableAsync<T>(ObservableCollection<T> collection, Func<Task<List<T>>> source)
        {
            IsBusy = true;

            try
            {
                collection.Clear();
                //var items = await LocalAppDatabase.GetAuditTypeAsync();
                foreach (T item in await source.Invoke())
                {
                    collection.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        protected bool SetProperty<T>(Action<T> setAction, Func<T> getAction, T value,
           [CallerMemberName] string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(getAction.Invoke(), value))
                return false;

            setAction.Invoke(value);
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        //#region INotifyPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;
        ////protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        ////{
        ////    var changed = PropertyChanged;
        ////    if (changed == null)
        ////        return;

        ////    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        ////}
        //#endregion
















        /// <summary>
        /// MR new changes
        /// </summary>


        //protected readonly IConnectionService _connectionService;
        protected readonly INavigationService _navigationService;
        //protected readonly IDialogService _dialogService;


        public BaseViewModel(
            //IConnectionService connectionService, 
            INavigationService navigationService
            //,IDialogService dialogService
            )
        {
            //_connectionService = connectionService;
            _navigationService = navigationService;
            //_dialogService = dialogService;
        }

        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task InitializeAsync(object data)
        {
            return Task.FromResult(false);
        }


        //protected virtual void OnPropertyChanged(string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //public virtual void Initialize(object parameter)
        //{

        //}

        /// <summary>
        /// //false is default value when system call back press
        /// </summary>
        /// <returns></returns>
        public virtual bool OnBackButtonPressed()
        {
            //false is default value when system call back press
            return false;
        }


        //protected virtual bool OnBackButtonPressed();

        /// <summary>
        /// called when page need override soft back button
        /// </summary>
        public virtual void OnSoftBackButtonPressed() { }

        protected virtual void OnDisappearing() { }

    }


}
