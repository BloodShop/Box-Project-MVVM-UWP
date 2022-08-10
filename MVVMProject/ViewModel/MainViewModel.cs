using DAL;
using Model;
using Model.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace MVVMProject.ViewModel
{ // Encapsulate the Concept that Varies // Favor Composition over Inheritence
    public class MainViewModel : MainViewModelBase
    {
        static BoxesBST _boxBST = new BoxesBST();
        public ObservableCollection<Box> PurchaseBoxes { get; private set; } = new ObservableDoublyLinkedQueue<Box>();
        public ObservableCollection<Box> QueueBoxes { get; private set; } = new DoublyLinkedQueue<Box>();
        public ObservableCollection<Box> AllBoxes { get; private set; } = new DoublyLinkedQueue<Box>();
        public Box SelectedBox { get; set; }

        public MainViewModel() => InitListViews();
        void InitListViews() 
        {
            QueueBoxes?.Clear();
            AllBoxes?.Clear();
            foreach (Box boxQ in _boxBST.DateQ)
                QueueBoxes.Add(boxQ);
            
            foreach (Box box in _boxBST)
                AllBoxes.Add(box);
        }

        #region Props
        #region Search Terms command
        public bool IsSearchValid
        {
            get => _isSearchValid;
            private set
            {
                _isSearchValid = value;
                RaisePropertyChanged(nameof(IsSearchValid));
            }
        }
        bool _isSearchValid = false;

        double _searchWidth;
        double _searchHeight;
        int _searchAmount;
        public double SearchWidth
        {
            get => _searchWidth;
            set
            {
                _searchWidth = value;
                if (_searchWidth > 0 && _searchHeight > 0 && _searchAmount > 0) IsSearchValid = true;
                RaisePropertyChanged(nameof(SearchWidth));
            }
        }
        public double SearchHeight
        {
            get => _searchHeight;
            set
            {
                _searchHeight = value;
                if (_searchWidth > 0 && _searchHeight > 0 && _searchAmount > 0) IsSearchValid = true;
                RaisePropertyChanged(nameof(SearchHeight));
            }
        }
        public int SearchAmount
        {
            get => _searchAmount;
            set
            {
                _searchAmount = value;
                if (_searchWidth > 0 && _searchHeight > 0 && _searchAmount > 0) IsSearchValid = true;
                RaisePropertyChanged(nameof(SearchAmount));
            }
        }
        #endregion
        #region Amount of boxes to be removed from stock - Binded with TextBox
        public int RemoveAmount
        {
            get => _removeAmount;
            set
            {
                _removeAmount = value;
                if (_removeAmount > 0 && SelectedBox != null && _removeAmount <= SelectedBox?.Amount) IsRemoveValid = true;
                RaisePropertyChanged(nameof(RemoveAmount));
            }
        }
        public int _removeAmount;
        bool _isRemoveValid = false;
        public bool IsRemoveValid
        {
            get => _isRemoveValid;
            private set
            {
                _isRemoveValid = value;
                RaisePropertyChanged(nameof(IsRemoveValid));
            }
        }
        #endregion
        #region Add Terms command
        bool _isAddValid = false;
        public bool IsAddValid
        {
            get => _isAddValid;
            private set
            {
                _isAddValid = value;
                RaisePropertyChanged(nameof(IsAddValid));
            }
        }
        public int AddAmount
        {
            get => _addAmount;
            set
            {
                if (value > 0) _addAmount = value;
                if (_addWidth > 0 && _addHeight > 0 && _addAmount > 0) IsAddValid = true;
                RaisePropertyChanged(nameof(AddAmount));
            }
        }
        int _addAmount;
        public int AmountBought
        {
            get => _amountBought;
            set
            {
                _amountBought = value;
                RaisePropertyChanged(nameof(AmountBought));
            }
        }
        int _amountBought;
        public double AddWidth
        {
            get => _addWidth;
            set
            {
                _addWidth = value;
                if (_addWidth > 0 && _addHeight > 0 && _addAmount > 0) IsAddValid = true;
                RaisePropertyChanged(nameof(AddWidth));
            }
        }
        double _addWidth;
        public double AddHeight
        {
            get => _addHeight;
            set
            {
                _addHeight = value;
                if (_addWidth > 0 && _addHeight > 0 && _addAmount > 0) IsAddValid = true;
                RaisePropertyChanged(nameof(AddHeight));
            }
        }
        double _addHeight;
        //public DateTime LastUsedDate
        //{
        //    get => _lastUsedDate;
        //    set
        //    {
        //        _lastUsedDate = value;
        //        RaisePropertyChanged(nameof(LastUsedDate));
        //    }
        //}
        //DateTime _lastUsedDate;
        //public DateTime ExpirationDate
        //{
        //    get => _expirationDate;
        //    set
        //    {
        //        if (DateDifference < 60) _expirationDate = value;
        //        RaisePropertyChanged(nameof(ExpirationDate));
        //    }
        //}
        //DateTime _expirationDate;
        //public int DateDifference
        //{
        //    get => (_expirationDate - _lastUsedDate).Days;
        //    set
        //    {
        //        _dateDifference = (_expirationDate - _lastUsedDate).Days;
        //        RaisePropertyChanged(nameof(DateDifference));
        //    }
        //}
        //int _dateDifference;
        #endregion
        #endregion

        public ICommand AddCommand => new DelegateCommand(AddBox);
        public ICommand RemoveCommand => new DelegateCommand(RemoveBox);
        public ICommand SearchCommand => new DelegateCommand(SearchBoxes);

        public void AddBox()
        {
            if (_addAmount > _boxBST.MAX_AMOUNT_BOXES)
            {
                Message($"Cannot add more boxes than maximum\n{_addAmount - _boxBST.MAX_AMOUNT_BOXES} boxes were Retrieved", "Back Supply");
                _addAmount = _boxBST.MAX_AMOUNT_BOXES;
            }
            _boxBST.Add(new Box(_addWidth, _addHeight, _addAmount));
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            IsAddValid = false;
            InitListViews();
        }
        public void RemoveBox()
        {
            if (SelectedBox != null && _removeAmount > 0 && _removeAmount <= SelectedBox.Amount)
            {
                _boxBST.Remove(SelectedBox, _removeAmount);
                IsRemoveValid = false;
                InitListViews();
            }
        }
        async void SearchBoxes()
        {
            try
            {
                //PurchaseBoxes?.Clear();
                foreach (Box b in _boxBST.Get(_searchWidth, _searchHeight, _searchAmount))
                {
                    PurchaseBoxes?.Add(b/*, (x) => x.DateDifference < b.DateDifference*/);
                    //RaisePropertyChanged(nameof(Box));
                }

                IUICommand resultDialog = await VerifyPurchase();
                if (resultDialog.Label == "No")
                    _boxBST.Retrieve();
                else
                {
                    foreach (Box box in PurchaseBoxes)
                    {
                        if (box.WarningQnt(5)) Message(box.ToString(), "Warning quantity");
                        //if (box.Amount == 0) 
                        //else RaisePropertyChanged(nameof(Box));
                    }
                    IsSearchValid = false;
                    InitListViews();
                }
            }
            catch (Exception ex)
            {
                _boxBST.Retrieve();
                await Message(ex.Message, "Error 1");
            }
        }
        async Task Message(string message, string title) => await new MessageDialog(message, title).ShowAsync();
        async Task<IUICommand> VerifyPurchase() // MessageDialog Yes/No verification before deleteing an Item
        {
            MessageDialog dialog = new MessageDialog("Are you sure?", "Purchase Items");
            dialog.Commands.Add(new UICommand("Yes"));
            dialog.Commands.Add(new UICommand("No"));
            return await dialog.ShowAsync();
        }
    }
}