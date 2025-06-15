using var game = new Venefica.Venefica();
game.IsFixedTimeStep = false;
//double FPS = 120;
//game.TargetElapsedTime = System.TimeSpan.FromSeconds(1 / FPS);
game.Run();
