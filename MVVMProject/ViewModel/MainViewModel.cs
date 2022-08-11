using DAL;
using Model;
using Model.DataStructures;
using Model.DataStructures.Observable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace MVVMProject.ViewModel
{ // Encapsulate the Concept that Varies 
    public class MainViewModel : MainViewModelBase
    {
        readonly static DispatcherTimer timer = new DispatcherTimer();
        readonly static DispatcherTimer QueueTimer = new DispatcherTimer();
        static DateTime _timeToday = DateTime.Now;
        public DateTime TimeToday
        {
            get => _timeToday;
            private set
            {
                _timeToday = value;
                RaisePropertyChanged(nameof(TimeToday));
            }
        }

        /// <summary>
        /// The store that manages the boxes with minimum Time complexity
        /// </summary>
        static BoxesBST _boxBST = new BoxesBST();
        /// <summary>
        /// List of 'wanted' boxes which the user wants to purchase --> Binded with GetPurchaseLV (ListView[xaml])
        /// </summary>
        public ObservableCollection<Box> PurchaseBoxes { get; private set; } = new DoublyLinkedQueue<Box>();
        /// <summary>
        /// List of boxes which has an exipiration date --> Binded with QueueDateLV (ListView[xaml])
        /// </summary>
        public ObservableCollection<Box> DateQueueBoxes { get; private set; } = new DoublyLinkedQueue<Box>();
        /// <summary>
        /// List of all boxes in store --> Binded with BoxesLV (ListView[xaml])
        /// </summary>
        public ObservableCollection<Box> AllBoxes { get; private set; } = new DoublyLinkedQueue<Box>();
        /// <summary>
        /// The selectedBox in the BoxesLV (AllBoxes) which the user wants to remove
        /// </summary>
        public Box SelectedBox { get; set; }

        public MainViewModel()
        {
            Init_Timer();
            InitListViews();
        }
        void InitListViews() // Init the listViews
        {
            DateQueueBoxes?.Clear();
            AllBoxes?.Clear();
            foreach (Box boxQ in _boxBST.DateQ)
                DateQueueBoxes.Add(boxQ);

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
        #endregion
        #endregion

        // Buttons command
        public ICommand AddCommand => new DelegateCommand(AddBox);
        public ICommand RemoveCommand => new DelegateCommand(RemoveBox);
        public ICommand SearchCommand => new DelegateCommand(SearchBoxes);
        public ICommand ExitCommnd => new DelegateCommand(ExitSave);

        void ExitSave()
        {
            DataBase.SaveDataBaseJson(AllBoxes);
            Application.Current.Exit();
        }
        /// <summary>
        /// Add a box to the repository by filling the Amount,Width and height
        /// </summary>
        void AddBox()
        {
            if (_boxBST.Contains(_addWidth, _addHeight, out Box box))
            {
                if (_addAmount + box.Amount > _boxBST.MAX_AMOUNT_BOXES)
                {
                    Message($"Cannot add more boxes than maximum\n{_addAmount + box.Amount - _boxBST.MAX_AMOUNT_BOXES} boxes were Retrieved", "Back Supply");
                    AddAmount = _boxBST.MAX_AMOUNT_BOXES - box.Amount;
                }
                _boxBST.AddToExicting(box, _addAmount);
            }
            else
            {
                if (_addAmount > _boxBST.MAX_AMOUNT_BOXES)
                {
                    Message($"Cannot add more boxes than maximum\n{_addAmount - _boxBST.MAX_AMOUNT_BOXES} boxes were Retrieved", "Back Supply");
                    AddAmount = _boxBST.MAX_AMOUNT_BOXES;
                }
                _boxBST.Add(new Box(_addWidth, _addHeight, _addAmount));
            }
            
            InitListViews();
        }
        /// <summary>
        /// Removes the selected box from the repository by filling Amount (To remove) and selecting Box
        /// </summary>
        void RemoveBox()
        {
            if (SelectedBox != null && _removeAmount > 0 && _removeAmount <= SelectedBox.Amount)
            {
                _boxBST.Remove(SelectedBox, _removeAmount);
                IsRemoveValid = false;
                InitListViews();
            }
        }
        /// <summary>
        /// Searches the box by the dimensions you ask for by filling Amount,Width, and Height
        /// </summary>
        async void SearchBoxes()
        {
            try
            {
                PurchaseBoxes?.Clear();
                foreach (Box b in _boxBST.Get(_searchWidth, _searchHeight, _searchAmount))
                    PurchaseBoxes?.Add(b);

                IUICommand resultDialog = await VerifyMessage();
                if (resultDialog.Label == "No")
                    throw new Exception("Offer was declined");
                else
                {
                    foreach (Box box in PurchaseBoxes)
                        if (box.WarningQnt(5)) Message(box.ToString(), "Warning quantity");
                    InitListViews();
                }
            }
            catch (Exception ex)
            {
                _boxBST.Retrieve();
                await Message(ex.Message, "Error 1");
            }
        }
        /// <summary>
        /// Represent a message to screen
        /// </summary>
        /// <param name="message">The message you want to show</param>
        /// <param name="title">The Title you want to show</param>
        /// <returns></returns>

        async Task Message(string message, string title) => await new MessageDialog(message, title).ShowAsync();
        /// <summary>
        /// Message with yes and no commands
        /// </summary>
        /// <returns></returns>
        async Task<IUICommand> VerifyMessage() // MessageDialog Yes/No verification before deleteing an Item
        {
            MessageDialog dialog = new MessageDialog("Are you sure?", "Purchase Items");
            dialog.Commands.Add(new UICommand("Yes"));
            dialog.Commands.Add(new UICommand("No"));
            return await dialog.ShowAsync();
        }

        void Init_Timer() // Initialize Timer when app loaded
        {
            QueueTimer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Interval = new TimeSpan(0, 0, 10, 0);
            QueueTimer.Tick += ShowTime_Tick;
            timer.Tick += ManageTmr_Tick;
            QueueTimer.Start();
            timer.Start();
        }
        private void ShowTime_Tick(object sender, object e) => TimeToday = DateTime.Now;

        void ManageTmr_Tick(object sender, object e) // Deletes front box if DateDiffernce is 0 - every 24 hours
        {
            if (_boxBST.DateQ.IsEmpty()) return;
            var qNode = _boxBST.DateQ.Peek().SelfRefrence;
            while (!_boxBST.DateQ.IsEmpty() && (DateTime.Now - qNode.Data.LastUsedDate).Days >= _boxBST.DAYS_TO_EXPIRE)
            {
                _boxBST.Remove((Box)_boxBST.DateQ.DeQueue().Clone(), int.MaxValue);
                qNode = qNode.Next;
            }
            InitListViews();
        }
    }
}