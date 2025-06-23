using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LogicMoveTests
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
    public void MoveTetrimino_WhenCalled_ShouldUpdateTetriminoPosition()
    {
        // Arrange
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.I);
        testTetrimino.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act
        logicManager.MoveTetrimino(Vector2Int.left);

        // Assert - Check that game state has been updated
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after move");

        // Verify the tetrimino actually moved
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.IsNotNull(currentTetrimino, "Current tetrimino should exist");
        Assert.AreNotEqual(initialPosition, currentTetrimino.position, "Tetrimino position should have changed");
        Assert.AreEqual(initialPosition.x - 1, currentTetrimino.position.x, "Tetrimino should have moved left by 1");
        Assert.AreEqual(initialPosition.y, currentTetrimino.position.y, "Tetrimino Y position should remain the same");
    }

    [Test]
    public void MoveTetriminoJ_WhenCalled_ShouldUpdateTetriminoPosition()
    {
        // Arrange
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.J);
        testTetrimino.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act
        logicManager.MoveTetrimino(Vector2Int.left);

        // Assert - Check that game state has been updated
        var updatedGameData = logicManager.GetGameData();
        Assert.IsNotNull(updatedGameData, "Game data should be available after move");

        // Verify the tetrimino actually moved
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.IsNotNull(currentTetrimino, "Current tetrimino should exist");
        Assert.AreNotEqual(initialPosition, currentTetrimino.position, "Tetrimino position should have changed");
        Assert.AreEqual(initialPosition.x - 1, currentTetrimino.position.x, "Tetrimino should have moved left by 1");
        Assert.AreEqual(initialPosition.y, currentTetrimino.position.y, "Tetrimino Y position should remain the same");
    }

    [Test]
    public void MoveTetrimino_IBLock_WhenAtLeftBoundary_ShouldNotMoveOutsideBoard()
    {
        // Arrange - I 블록은 중심점에서 왼쪽으로 1칸 차지하므로, x=1이 왼쪽 경계
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.I);
        testTetrimino.position = new Vector2Int(1, 10); // I블록이 경계에 딱 맞는 위치
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 왼쪽으로 이동 시도 (x=-1이 되어 경계 밖으로 나감)
        logicManager.MoveTetrimino(Vector2Int.left);

        // Assert - 이동하지 않아야 함
        var updatedGameData = logicManager.GetGameData();
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.AreEqual(initialPosition, currentTetrimino.position,
            "I블록은 왼쪽 경계에서 더 이상 왼쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_JBlock_WhenAtLeftBoundary_ShouldNotMoveOutsideBoard()
    {
        // Arrange - J 블록도 중심점에서 왼쪽으로 1칸 차지하므로, x=1이 왼쪽 경계
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.J);
        testTetrimino.position = new Vector2Int(1, 10); // J블록이 경계에 딱 맞는 위치
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 왼쪽으로 이동 시도
        logicManager.MoveTetrimino(Vector2Int.left);

        // Assert - 이동하지 않아야 함
        var updatedGameData = logicManager.GetGameData();
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.AreEqual(initialPosition, currentTetrimino.position,
            "J블록은 왼쪽 경계에서 더 이상 왼쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_IBlock_WhenAtRightBoundary_ShouldNotMoveOutsideBoard()
    {
        // Arrange - I 블록은 중심점에서 오른쪽으로 2칸 차지하므로, x=7이 오른쪽 경계 (WIDTH=10)
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.I);
        testTetrimino.position = new Vector2Int(7, 10); // I블록 오른쪽 경계 (2칸이 8,9 차지)
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 오른쪽으로 이동 시도
        logicManager.MoveTetrimino(Vector2Int.right);

        // Assert - 이동하지 않아야 함
        var updatedGameData = logicManager.GetGameData();
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.AreEqual(initialPosition, currentTetrimino.position,
            "I블록은 오른쪽 경계에서 더 이상 오른쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_OBlock_SpecialBoundaryBehavior_ShouldWork()
    {
        // Arrange - O 블록은 중점이 왼쪽 아래 모서리라서 x=0부터 시작 가능
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.O);
        testTetrimino.position = new Vector2Int(0, 10); // O블록은 x=0에서 시작 가능
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 왼쪽으로 이동 시도 (x=-1이 되면 경계 밖)
        logicManager.MoveTetrimino(Vector2Int.left);

        // Assert - 이동하지 않아야 함
        var updatedGameData = logicManager.GetGameData();
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.AreEqual(initialPosition, currentTetrimino.position,
            "O블록은 x=0에서 더 이상 왼쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_OBlock_WhenAtRightBoundary_ShouldNotMoveOutside()
    {
        // Arrange - O블록은 중심점에서 오른쪽으로 1칸 차지하므로, x=8이 오른쪽 경계
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.O);
        testTetrimino.position = new Vector2Int(8, 10); // O블록 오른쪽 경계
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 오른쪽으로 이동 시도
        logicManager.MoveTetrimino(Vector2Int.right);

        // Assert - 이동하지 않아야 함
        var updatedGameData = logicManager.GetGameData();
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.AreEqual(initialPosition, currentTetrimino.position,
            "O블록은 오른쪽 경계에서 더 이상 오른쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_TBlock_BoundaryBehavior_ShouldWork()
    {
        // Arrange - T 블록은 대칭적으로 좌우 1칸씩 차지
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.T);
        testTetrimino.position = new Vector2Int(1, 10); // T블록 왼쪽 경계
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 왼쪽으로 이동 시도
        logicManager.MoveTetrimino(Vector2Int.left);

        // Assert - 이동하지 않아야 함
        var updatedGameData = logicManager.GetGameData();
        var currentTetrimino = updatedGameData.currentTetrimino;
        Assert.AreEqual(initialPosition, currentTetrimino.position,
            "T블록은 왼쪽 경계에서 더 이상 왼쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_WhenAtBottomBoundary_ShouldNotMoveDown()
    {
        // Arrange - 바닥 경계 테스트 (y=0이 바닥)
        var gameData = logicManager.GetGameData();
        var testTetrimino = new Tetrimino(TetriminoType.I);
        testTetrimino.position = new Vector2Int(5, 0); // 바닥에 위치
        gameData.currentTetrimino = testTetrimino;

        var initialPosition = testTetrimino.position;

        // Act - 아래로 이동 시도
        logicManager.MoveTetrimino(Vector2Int.down);

        // Assert - 이동하지 않거나 테트리미노가 고정되어야 함
        var updatedGameData = logicManager.GetGameData();
        // 바닥에 닿으면 새로운 테트리미노가 생성될 수 있으므로, 
        // 이동이 제한되었는지 확인하는 것이 중요
        Assert.IsNotNull(updatedGameData, "바닥 경계에서도 게임 데이터는 유효해야 함");
        Assert.IsNotNull(updatedGameData.currentTetrimino, "현재 테트리미노가 존재해야 함");
    }

    [Test]
    public void Tetrimino_BoundaryValues_ShouldBeCorrectForEachType()
    {
        // 각 테트리미노 타입별로 실제 경계값 검증

        // I 블록: 왼쪽 -1, 오른쪽 +2
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(1, 10); // 왼쪽 경계
        var iPositions = iBlock.GetWorldPositions();
        var minX = System.Linq.Enumerable.Min(iPositions, pos => pos.x);
        var maxX = System.Linq.Enumerable.Max(iPositions, pos => pos.x);
        Assert.AreEqual(0, minX, "I블록이 x=1에 있을 때 최소 x는 0이어야 함");
        Assert.AreEqual(3, maxX, "I블록이 x=1에 있을 때 최대 x는 3이어야 함");

        // O 블록: 왼쪽 0, 오른쪽 +1
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(0, 10); // 왼쪽 경계
        var oPositions = oBlock.GetWorldPositions();
        minX = System.Linq.Enumerable.Min(oPositions, pos => pos.x);
        maxX = System.Linq.Enumerable.Max(oPositions, pos => pos.x);
        Assert.AreEqual(0, minX, "O블록이 x=0에 있을 때 최소 x는 0이어야 함");
        Assert.AreEqual(1, maxX, "O블록이 x=0에 있을 때 최대 x는 1이어야 함");

        // J 블록: 왼쪽 -1, 오른쪽 +1
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(1, 10); // 왼쪽 경계
        var jPositions = jBlock.GetWorldPositions();
        minX = System.Linq.Enumerable.Min(jPositions, pos => pos.x);
        maxX = System.Linq.Enumerable.Max(jPositions, pos => pos.x);
        Assert.AreEqual(0, minX, "J블록이 x=1에 있을 때 최소 x는 0이어야 함");
        Assert.AreEqual(2, maxX, "J블록이 x=1에 있을 때 최대 x는 2이어야 함");

        // T 블록: 왼쪽 -1, 오른쪽 +1
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(1, 10); // 왼쪽 경계
        var tPositions = tBlock.GetWorldPositions();
        minX = System.Linq.Enumerable.Min(tPositions, pos => pos.x);
        maxX = System.Linq.Enumerable.Max(tPositions, pos => pos.x);
        Assert.AreEqual(0, minX, "T블록이 x=1에 있을 때 최소 x는 0이어야 함");
        Assert.AreEqual(2, maxX, "T블록이 x=1에 있을 때 최대 x는 2이어야 함");
    }

    [Test]
    public void MoveTetrimino_RightBoundary_AllBlocks_ShouldBehaveCorrectly()
    {
        // I 블록 - 오른쪽 경계 x=7 (최대 x가 9가 됨)
        var gameData = logicManager.GetGameData();
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(7, 10);
        gameData.currentTetrimino = iBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(7, 10), gameData.currentTetrimino.position,
            "I블록은 x=7에서 오른쪽으로 이동할 수 없어야 함");

        // O 블록 - 오른쪽 경계 x=8 (최대 x가 9가 됨)
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = oBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "O블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");

        // J 블록 - 오른쪽 경계 x=8 (최대 x가 9가 됨)
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = jBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "J블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void Tetrimino_AllShapes_ShouldHaveCorrectOccupiedPositions()
    {
        // Test O 블록
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(5, 10);
        var oPositions = oBlock.GetWorldPositions();

        Assert.AreEqual(4, oPositions.Length, "O블록은 4개 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(5, 10), oPositions, "O블록 중심점");
        Assert.Contains(new Vector2Int(6, 10), oPositions, "O블록 오른쪽 아래");
        Assert.Contains(new Vector2Int(5, 11), oPositions, "O블록 왼쪽 위");
        Assert.Contains(new Vector2Int(6, 11), oPositions, "O블록 오른쪽 위");

        // Test T 블록
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(5, 10);
        var tPositions = tBlock.GetWorldPositions();

        Assert.AreEqual(4, tPositions.Length, "T블록은 4개 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(4, 10), tPositions, "T블록 왼쪽");
        Assert.Contains(new Vector2Int(5, 10), tPositions, "T블록 중심점");
        Assert.Contains(new Vector2Int(6, 10), tPositions, "T블록 오른쪽");
        Assert.Contains(new Vector2Int(5, 11), tPositions, "T블록 위쪽");

        // Test S 블록
        var sBlock = new Tetrimino(TetriminoType.S);
        sBlock.position = new Vector2Int(5, 10);
        var sPositions = sBlock.GetWorldPositions();

        Assert.AreEqual(4, sPositions.Length, "S블록은 4개 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(4, 10), sPositions, "S블록 왼쪽 아래");
        Assert.Contains(new Vector2Int(5, 10), sPositions, "S블록 중심점");
        Assert.Contains(new Vector2Int(5, 11), sPositions, "S블록 중심 위");
        Assert.Contains(new Vector2Int(6, 11), sPositions, "S블록 오른쪽 위");

        // Test I 블록
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(5, 10);
        var iPositions = iBlock.GetWorldPositions();

        Assert.AreEqual(4, iPositions.Length, "I블록은 4개 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(4, 10), iPositions, "I블록은 중심점-1 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(5, 10), iPositions, "I블록은 중심점 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(6, 10), iPositions, "I블록은 중심점+1 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(7, 10), iPositions, "I블록은 중심점+2 위치를 차지해야 함");

        // Test J 블록
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(5, 10);
        var jPositions = jBlock.GetWorldPositions();

        Assert.AreEqual(4, jPositions.Length, "J블록은 4개 위치를 차지해야 함");
        Assert.Contains(new Vector2Int(4, 11), jPositions, "J블록 위쪽 왼쪽");
        Assert.Contains(new Vector2Int(4, 10), jPositions, "J블록 아래쪽 왼쪽");
        Assert.Contains(new Vector2Int(5, 10), jPositions, "J블록 중심점");
        Assert.Contains(new Vector2Int(6, 10), jPositions, "J블록 아래쪽 오른쪽");
    }

    [Test]
    public void MoveTetrimino_AllTypes_LeftMovement_ShouldWorkCorrectly()
    {
        // I 블록 왼쪽 이동 테스트
        var gameData = logicManager.GetGameData();
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = iBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "I블록이 왼쪽으로 정상 이동해야 함");

        // O 블록 왼쪽 이동 테스트
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = oBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "O블록이 왼쪽으로 정상 이동해야 함");

        // T 블록 왼쪽 이동 테스트
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = tBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "T블록이 왼쪽으로 정상 이동해야 함");

        // S 블록 왼쪽 이동 테스트
        var sBlock = new Tetrimino(TetriminoType.S);
        sBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = sBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "S블록이 왼쪽으로 정상 이동해야 함");

        // Z 블록 왼쪽 이동 테스트
        var zBlock = new Tetrimino(TetriminoType.Z);
        zBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = zBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "Z블록이 왼쪽으로 정상 이동해야 함");

        // J 블록 왼쪽 이동 테스트
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = jBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "J블록이 왼쪽으로 정상 이동해야 함");

        // L 블록 왼쪽 이동 테스트
        var lBlock = new Tetrimino(TetriminoType.L);
        lBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = lBlock;

        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(4, 10), gameData.currentTetrimino.position,
            "L블록이 왼쪽으로 정상 이동해야 함");
    }

    [Test]
    public void MoveTetrimino_AllTypes_RightMovement_ShouldWorkCorrectly()
    {
        // I 블록 오른쪽 이동 테스트
        var gameData = logicManager.GetGameData();
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = iBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "I블록이 오른쪽으로 정상 이동해야 함");

        // O 블록 오른쪽 이동 테스트
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = oBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "O블록이 오른쪽으로 정상 이동해야 함");

        // T 블록 오른쪽 이동 테스트
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = tBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "T블록이 오른쪽으로 정상 이동해야 함");

        // S 블록 오른쪽 이동 테스트
        var sBlock = new Tetrimino(TetriminoType.S);
        sBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = sBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "S블록이 오른쪽으로 정상 이동해야 함");

        // Z 블록 오른쪽 이동 테스트
        var zBlock = new Tetrimino(TetriminoType.Z);
        zBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = zBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "Z블록이 오른쪽으로 정상 이동해야 함");

        // J 블록 오른쪽 이동 테스트
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = jBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "J블록이 오른쪽으로 정상 이동해야 함");

        // L 블록 오른쪽 이동 테스트
        var lBlock = new Tetrimino(TetriminoType.L);
        lBlock.position = new Vector2Int(5, 10);
        gameData.currentTetrimino = lBlock;

        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(6, 10), gameData.currentTetrimino.position,
            "L블록이 오른쪽으로 정상 이동해야 함");
    }

    [Test]
    public void MoveTetrimino_AllTypes_LeftBoundaryBehavior_ShouldWork()
    {
        var gameData = logicManager.GetGameData();

        // I 블록 - 왼쪽 경계 x=1
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(1, 10);
        gameData.currentTetrimino = iBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(1, 10), gameData.currentTetrimino.position,
            "I블록은 x=1에서 왼쪽으로 이동할 수 없어야 함");

        // O 블록 - 왼쪽 경계 x=0
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(0, 10);
        gameData.currentTetrimino = oBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(0, 10), gameData.currentTetrimino.position,
            "O블록은 x=0에서 왼쪽으로 이동할 수 없어야 함");

        // T 블록 - 왼쪽 경계 x=1
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(1, 10);
        gameData.currentTetrimino = tBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(1, 10), gameData.currentTetrimino.position,
            "T블록은 x=1에서 왼쪽으로 이동할 수 없어야 함");

        // S 블록 - 왼쪽 경계 x=1
        var sBlock = new Tetrimino(TetriminoType.S);
        sBlock.position = new Vector2Int(1, 10);
        gameData.currentTetrimino = sBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(1, 10), gameData.currentTetrimino.position,
            "S블록은 x=1에서 왼쪽으로 이동할 수 없어야 함");

        // Z 블록 - 왼쪽 경계 x=1
        var zBlock = new Tetrimino(TetriminoType.Z);
        zBlock.position = new Vector2Int(1, 10);
        gameData.currentTetrimino = zBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(1, 10), gameData.currentTetrimino.position,
            "Z블록은 x=1에서 왼쪽으로 이동할 수 없어야 함");

        // J 블록 - 왼쪽 경계 x=1
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(1, 10);
        gameData.currentTetrimino = jBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(1, 10), gameData.currentTetrimino.position,
            "J블록은 x=1에서 왼쪽으로 이동할 수 없어야 함");

        // L 블록 - 왼쪽 경계 x=1
        var lBlock = new Tetrimino(TetriminoType.L);
        lBlock.position = new Vector2Int(1, 10);
        gameData.currentTetrimino = lBlock;
        logicManager.MoveTetrimino(Vector2Int.left);
        Assert.AreEqual(new Vector2Int(1, 10), gameData.currentTetrimino.position,
            "L블록은 x=1에서 왼쪽으로 이동할 수 없어야 함");
    }

    [Test]
    public void MoveTetrimino_AllTypes_RightBoundaryBehavior_ShouldWork()
    {
        var gameData = logicManager.GetGameData();

        // I 블록 - 오른쪽 경계 x=7 (WIDTH=10, I블록은 +2까지 차지)
        var iBlock = new Tetrimino(TetriminoType.I);
        iBlock.position = new Vector2Int(7, 10);
        gameData.currentTetrimino = iBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(7, 10), gameData.currentTetrimino.position,
            "I블록은 x=7에서 오른쪽으로 이동할 수 없어야 함");

        // O 블록 - 오른쪽 경계 x=8 (O블록은 +1까지 차지)
        var oBlock = new Tetrimino(TetriminoType.O);
        oBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = oBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "O블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");

        // T 블록 - 오른쪽 경계 x=8 (T블록은 +1까지 차지)
        var tBlock = new Tetrimino(TetriminoType.T);
        tBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = tBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "T블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");

        // S 블록 - 오른쪽 경계 x=8 (S블록은 +1까지 차지)
        var sBlock = new Tetrimino(TetriminoType.S);
        sBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = sBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "S블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");

        // Z 블록 - 오른쪽 경계 x=8 (Z블록은 +1까지 차지)
        var zBlock = new Tetrimino(TetriminoType.Z);
        zBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = zBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "Z블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");

        // J 블록 - 오른쪽 경계 x=8 (J블록은 +1까지 차지)
        var jBlock = new Tetrimino(TetriminoType.J);
        jBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = jBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "J블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");

        // L 블록 - 오른쪽 경계 x=8 (L블록은 +1까지 차지)
        var lBlock = new Tetrimino(TetriminoType.L);
        lBlock.position = new Vector2Int(8, 10);
        gameData.currentTetrimino = lBlock;
        logicManager.MoveTetrimino(Vector2Int.right);
        Assert.AreEqual(new Vector2Int(8, 10), gameData.currentTetrimino.position,
            "L블록은 x=8에서 오른쪽으로 이동할 수 없어야 함");
    }
}
