//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class MainMenuState : IGameState
//{
//    public void Enter()
//    {
//        Time.timeScale = 1;
//        GameUIManager.Instance.ShowHome();
//    }

//    public void Exit()
//    {
//        GameUIManager.Instance.HideAll();
//    }

//    public void Update() { }
//}

//public class GameplayState : IGameState
//{
//    private string sceneName;

//    public GameplayState(string scene)
//    {
//        sceneName = scene;
//    }

//    public void Enter()
//    {
//        Time.timeScale = 1;
//        GameUIManager.Instance.HideAll();
//        SceneManager.LoadScene(sceneName);
//    }

//    public void Exit() { }
//    public void Update() { }
//}

//public class PauseState : IGameState
//{
//    public void Enter()
//    {
//        Time.timeScale = 0;
//        GameUIManager.Instance.ShowPause();
//    }

//    public void Exit()
//    {
//        Time.timeScale = 1;
//        GameUIManager.Instance.HidePause();
//    }

//    public void Update() { }
//}

//public class GameOverState : IGameState
//{
//    public void Enter()
//    {
//        GameUIManager.Instance.ShowGameOver();
//    }

//    public void Exit()
//    {
//        GameUIManager.Instance.HideAll();
//    }

//    public void Update() { }
//}
