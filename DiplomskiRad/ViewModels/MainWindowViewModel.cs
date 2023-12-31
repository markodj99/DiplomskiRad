﻿using DiplomskiRad.Models.Enums;
using DiplomskiRad.Stores;

namespace DiplomskiRad.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set  => SetProperty(ref _currentViewModel, value);
        }
        private ColorSelectionViewModel _colorSelectionViewModel;
        private EngineStrengthViewModel _engineStrengthViewModel;
        private ChessBoardPuzzleViewModel _chessBoardPuzzleViewModel;
        private ChessBoardGameViewModel _chessBoardGameViewModel;

        private readonly ColorSelectionStore _colorSelectionStore;
        private readonly EngineStrengthStore _engineStrengthStore;
        private readonly EngineStrengthEvaluatedStore _engineStrengthEvaluatedStore;
        private readonly NewGameStore _newGameStore;

        public MainWindowViewModel(ColorSelectionViewModel colorSelectionViewModel, EngineStrengthViewModel engineStrengthViewModel,
            ChessBoardPuzzleViewModel chessBoardPuzzleViewModel, ChessBoardGameViewModel chessBoardGameViewModel,
            ColorSelectionStore colorSelectionStore, EngineStrengthStore engineStrengthStore, 
            EngineStrengthEvaluatedStore engineStrengthEvaluatedStore, NewGameStore newGameStore)
        {
            _colorSelectionStore = colorSelectionStore;
            _engineStrengthStore = engineStrengthStore;
            _engineStrengthEvaluatedStore = engineStrengthEvaluatedStore;
            _newGameStore = newGameStore;

            _colorSelectionViewModel = colorSelectionViewModel;
            _engineStrengthViewModel = engineStrengthViewModel;
            _chessBoardPuzzleViewModel = chessBoardPuzzleViewModel;
            _chessBoardGameViewModel = chessBoardGameViewModel;

            _colorSelectionStore.ColorSelected += ColorSelected;
            _engineStrengthStore.StrengthSelected += StrengthSelected;
            _engineStrengthEvaluatedStore.RatingEvaluated += StrengthEvaluated;
            _newGameStore.NewGameClicked += ClickedNewGame;

            CurrentViewModel = _colorSelectionViewModel;
        }

        private void ColorSelected(Color color)
        {
            _chessBoardGameViewModel.PlayerColor = color;
            CurrentViewModel = _engineStrengthViewModel;
        }

        private void StrengthSelected(int selectedStrength, RatingEvaluation evaluation)
        {
            if (evaluation == RatingEvaluation.AutoCalculated)
            {
                _chessBoardPuzzleViewModel.Start();
                CurrentViewModel = _chessBoardPuzzleViewModel;
            }
            else
            {
                _chessBoardGameViewModel.EngineStrength = selectedStrength;
                _chessBoardGameViewModel.Start();
                CurrentViewModel = _chessBoardGameViewModel;
            }
        }

        private void StrengthEvaluated(int evaluatedStrength)
        {
            _chessBoardGameViewModel.EngineStrength = evaluatedStrength;
            _chessBoardGameViewModel.Start();
            CurrentViewModel = _chessBoardGameViewModel;
        }

        private void ClickedNewGame() => CurrentViewModel = _colorSelectionViewModel;

        public override void Dispose()
        {
            _colorSelectionStore.ColorSelected -= ColorSelected;
            _engineStrengthStore.StrengthSelected -= StrengthSelected;
            _engineStrengthEvaluatedStore.RatingEvaluated -= StrengthEvaluated;
            _newGameStore.NewGameClicked -= ClickedNewGame;
        }
    }
}
