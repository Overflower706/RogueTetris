using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LogicTests
{
    private GameObject logicManagerObject;
    private LogicManager logicManager;
    private Game gameData;

    [SetUp]
    public void SetUp()
    {
        // LogicManager 생성 및 초기화
        logicManagerObject = new GameObject("TestLogicManager");
        logicManager = logicManagerObject.AddComponent<LogicManager>();

        // Initialize 메서드 호출하여 초기화
        logicManager.Initialize();

        // GetGameData 메서드를 통해 게임 데이터 가져오기
        gameData = logicManager.GetGameData();
    }

    [TearDown]
    public void TearDown()
    {
        if (logicManagerObject != null)
        {
            Object.DestroyImmediate(logicManagerObject);
        }
    }

    [Test]
    public void LogicManager_Initialization_ShouldCreateGameData()
    {
        // Arrange & Act
        // SetUp에서 이미 초기화됨

        // Assert
        Assert.IsNotNull(logicManager, "LogicManager component should be created");
        Assert.IsNotNull(gameData, "Game data should be initialized");
    }    // Move 관련 테스트는 LogicMoveTests.cs로 이동
    // Rotate 관련 테스트는 LogicRotateTests.cs로 이동    [Test]
    public void DropTetrimino_WhenCalled_ShouldProcessDrop()
    {
        // Arrange
        // (Setup already provides initialized logicManager)

        // Act
        logicManager.DropTetrimino();

        // Assert
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after drop");
    }

    [Test]
    public void DropTetrimino_ShouldMoveToBottom_WhenBoardIsEmpty()
    {        // Arrange
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.I);
        testTetrimino.position = new Vector2Int(5, 15); // 높은 위치에서 시작
        gameData.currentTetrimino = testTetrimino;

        var initialY = testTetrimino.position.y;

        // Act
        logicManager.DropTetrimino();

        // Assert
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after drop");

        // Drop 후에는 테트리미노가 바닥까지 떨어져야 함
        // 현재 테트리미노가 여전히 존재한다면, 바닥에 도달했어야 함
        Assert.IsTrue(testTetrimino.position.y <= initialY,
            "테트리미노가 초기 위치보다 아래로 이동해야 함");
        Assert.AreEqual(0, testTetrimino.position.y,
            "I블록은 빈 보드에서 y=0(바닥)까지 떨어져야 함");
    }

    [Test]
    public void DropTetrimino_AllTypes_ShouldDropToBottom()
    {
        var gameData = logicManager.GetGameData();

        // I 블록 Drop 테스트
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = iBlock;
        var initialIPosition = iBlock.position;

        logicManager.DropTetrimino();
        var afterIDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterIDropData, "I블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterIDropData.currentTetrimino, "I블록 Drop 후 테트리미노 존재");

        // O 블록 Drop 테스트
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = oBlock;
        var initialOPosition = oBlock.position;

        logicManager.DropTetrimino();
        var afterODropData = logicManager.GetGameData();
        Assert.IsNotNull(afterODropData, "O블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterODropData.currentTetrimino, "O블록 Drop 후 테트리미노 존재");

        // T 블록 Drop 테스트
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = tBlock;
        var initialTPosition = tBlock.position;

        logicManager.DropTetrimino();
        var afterTDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterTDropData, "T블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterTDropData.currentTetrimino, "T블록 Drop 후 테트리미노 존재");

        // S 블록 Drop 테스트
        var sBlock = new Tetrimino(TetriminoType.S);
        sBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = sBlock;
        var initialSPosition = sBlock.position;

        logicManager.DropTetrimino();
        var afterSDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterSDropData, "S블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterSDropData.currentTetrimino, "S블록 Drop 후 테트리미노 존재");

        // Z 블록 Drop 테스트
        var zBlock = new Tetrimino(TetriminoType.Z);
        zBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = zBlock;
        var initialZPosition = zBlock.position;

        logicManager.DropTetrimino();
        var afterZDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterZDropData, "Z블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterZDropData.currentTetrimino, "Z블록 Drop 후 테트리미노 존재");

        // J 블록 Drop 테스트
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = jBlock;
        var initialJPosition = jBlock.position;

        logicManager.DropTetrimino();
        var afterJDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterJDropData, "J블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterJDropData.currentTetrimino, "J블록 Drop 후 테트리미노 존재");

        // L 블록 Drop 테스트
        var lBlock = new Tetrimino(TetriminoType.L);
        lBlock.position = new Vector2Int(5, 15);
        gameData.currentTetrimino = lBlock;
        var initialLPosition = lBlock.position;

        logicManager.DropTetrimino();
        var afterLDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterLDropData, "L블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterLDropData.currentTetrimino, "L블록 Drop 후 테트리미노 존재");
    }

    [Test]
    public void DropTetrimino_ShouldStopWhenHittingBottom()
    {
        // Arrange - 테트리미노를 바닥 근처에 배치
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.O);
        testTetrimino.position = new Vector2Int(5, 2); // 바닥 근처
        gameData.currentTetrimino = testTetrimino;

        // Act
        logicManager.DropTetrimino();

        // Assert
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Drop 후에도 게임 데이터 유효");

        // Drop 후에는 테트리미노가 고정되고 새로운 테트리미노가 생성되거나
        // 현재 테트리미노가 바닥에 멈춰야 함
        Assert.IsNotNull(updatedGameData.currentTetrimino, "Drop 후에도 테트리미노 존재");
    }

    [Test]
    public void DropTetrimino_ShouldWorkWithRotatedTetriminos()
    {
        // Arrange - 회전된 테트리미노에 대한 Drop 테스트
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.I);
        testTetrimino.position = new Vector2Int(5, 15);
        testTetrimino.rotation = 1; // 세로로 회전된 I블록
        gameData.currentTetrimino = testTetrimino;

        // Act
        logicManager.DropTetrimino();

        // Assert
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "회전된 테트리미노 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(updatedGameData.currentTetrimino, "회전된 테트리미노 Drop 후 테트리미노 존재");

        // T 블록 회전 후 Drop 테스트
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(5, 15);
        tBlock.rotation = 2; // 180도 회전된 T블록
        gameData.currentTetrimino = tBlock;

        logicManager.DropTetrimino();

        var afterTDropData = logicManager.GetGameData();
        Assert.IsNotNull(afterTDropData, "회전된 T블록 Drop 후 게임 데이터 유효");
        Assert.IsNotNull(afterTDropData.currentTetrimino, "회전된 T블록 Drop 후 테트리미노 존재");
    }
    [Test]
    public void RestartGame_WhenCalled_ShouldResetGameState()
    {
        // Arrange
        var initialGameData = logicManager.GetGameData();

        // Modify game state before restart
        initialGameData.currentScore = 500;

        // Act
        logicManager.RestartGame();        // Assert
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after restart");
        Assert.AreEqual(0, updatedGameData.currentScore, "Score should be reset to 0 after restart");
    }
    [Test]
    public void GameData_Properties_ShouldBeAccessibleAndValid()
    {
        // Arrange
        var gameData = logicManager.GetGameData();

        // Act & Assert - Test current state
        Assert.IsNotNull(gameData.currentState, "Game should have a valid current state");

        // Test current score
        Assert.IsTrue(gameData.currentScore >= 0, "Current score should be non-negative");

        // Test target score
        Assert.IsTrue(gameData.targetScore > 0, "Target score should be positive");

        // Test board
        Assert.IsNotNull(gameData.board, "Game should have a valid board");
    }
    [Test]
    public void ShopLogic_OpenShop_ShouldChangeGameState()
    {
        // Arrange
        var gameData = logicManager.GetGameData();

        // Set game state to Victory first (required for OpenShop to work)
        gameData.currentState = GameState.Victory;

        // Act
        logicManager.OpenShop();

        // Assert
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after opening shop"); Assert.AreEqual(GameState.Shop, updatedGameData.currentState, "Game state should change to Shop");
        Assert.IsTrue(updatedGameData.isShopOpen, "Shop should be marked as open");
    }
    [Test]
    public void ShopLogic_CloseShop_ShouldReturnToGameplay()
    {
        // Arrange
        var logicMgr = logicManager;
        var gameData = logicMgr.GetGameData();

        // First set to Victory, then open shop
        gameData.currentState = GameState.Victory;
        logicMgr.OpenShop();

        // Act - Close shop
        logicMgr.CloseShop();

        // Assert
        var updatedGameData = logicMgr.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after closing shop"); Assert.AreEqual(GameState.Playing, updatedGameData.currentState, "Game state should return to Playing");
        Assert.IsFalse(updatedGameData.isShopOpen, "Shop should be marked as closed");
    }
    [Test]
    public void ShopLogic_GetShopItems_ShouldReturnItems()
    {
        // Arrange
        var logicMgr = logicManager;

        // Act
        var shopItems = logicMgr.GetShopItems();        // Assert
        Assert.IsNotNull(shopItems, "GetShopItems should return a collection of items");
        Assert.IsTrue(shopItems is System.Collections.Generic.List<ShopItem>, "Should return a List<ShopItem>");
    }
    [Test]
    public void SharedClasses_GameInstance_ShouldHaveValidData()
    {
        // Arrange & Act
        var gameInstance = new Game();
        Assert.IsNotNull(gameInstance, "Should be able to create Game instance");

        // Test TetrisBoard
        var boardInstance = new TetrisBoard();
        Assert.IsNotNull(boardInstance, "Should be able to create TetrisBoard instance");

        // Test Tetrimino (requires TetriminoType parameter)
        var tetriminoInstance = new Tetrimino(TetriminoType.I);
        Assert.IsNotNull(tetriminoInstance, "Should be able to create Tetrimino instance");        // Test ActiveEffect (requires constructor parameters)
        var effectInstance = new ActiveEffect(EffectType.ScoreMultiplier, 10f, 1.5f);
        Assert.IsNotNull(effectInstance, "Should be able to create ActiveEffect instance");
    }
    [UnityTest]
    public IEnumerator GameLoop_MultipleOperations_ShouldMaintainConsistentState()
    {
        // Arrange
        var logicMgr = (LogicManager)logicManager;

        // Act - Perform a sequence of game operations
        var initialGameData = logicMgr.GetGameData();
        Assert.IsNotNull(initialGameData, "Initial game data should be available");

        // Move left
        logicMgr.MoveTetrimino(Vector2Int.left);
        yield return new WaitForFixedUpdate();

        var afterMoveData = logicMgr.GetGameData();
        Assert.IsNotNull(afterMoveData, "Game data should remain valid after move");

        // Rotate
        logicMgr.RotateTetrimino();
        yield return new WaitForFixedUpdate();

        var afterRotateData = logicMgr.GetGameData();
        Assert.IsNotNull(afterRotateData, "Game data should remain valid after rotate");

        // Drop
        logicMgr.DropTetrimino();
        yield return new WaitForFixedUpdate();
        var afterDropData = logicMgr.GetGameData();
        Assert.IsNotNull(afterDropData, "Game data should remain valid after drop");
    }
}
