using FluentAssertions;

namespace Helpers.Tests;
public class TreeHelperTests
{
    [Fact]
    public void GetAllNodes_ShouldReturnAllNodes_WhenTreeIsProvided()
    {
        // Arrange
        var root = new TreeNode { Value = 1 };
        var child1 = new TreeNode { Value = 2 };
        var child2 = new TreeNode { Value = 3 };
        var grandchild1 = new TreeNode { Value = 4 };
        var grandchild2 = new TreeNode { Value = 5 };

        root.Children.Add(child1);
        root.Children.Add(child2);
        child1.Children.Add(grandchild1);
        child2.Children.Add(grandchild2);

        // Act
        var result = TreeHelper.GetAllNodes(root, node => node.Children);

        // Assert
        result.Should().BeEquivalentTo(new List<TreeNode> { root, child1, grandchild1, child2, grandchild2 },
            options => options.WithStrictOrdering());
    }

    [Fact]
    public void DoActionOnCondition_ShouldPerformActionOnMatchingNodes()
    {
        // Arrange
        var root = new TreeNode { Value = 1 };
        var child1 = new TreeNode { Value = 2 };
        var child2 = new TreeNode { Value = 3 };
        var grandchild1 = new TreeNode { Value = 4 };
        var grandchild2 = new TreeNode { Value = 5 };

        root.Children.Add(child1);
        root.Children.Add(child2);
        child1.Children.Add(grandchild1);
        child2.Children.Add(grandchild2);

        var collectedValues = new List<int>();

        // Act
        TreeHelper.DoActionOnCondition(
            root,
            node => node.Children,
            node => node.Value % 2 == 0, // Condition: Node value is even
            node => collectedValues.Add(node.Value)
        );

        // Assert
        collectedValues.Should().BeEquivalentTo(new List<int> { 2, 4 });
    }

    [Fact]
    public void DoActionOnCondition_ShouldPerformElseActionOnNonMatchingNodes()
    {
        // Arrange
        var root = new TreeNode { Value = 1 };
        var child1 = new TreeNode { Value = 2 };
        var child2 = new TreeNode { Value = 3 };
        var grandchild1 = new TreeNode { Value = 4 };
        var grandchild2 = new TreeNode { Value = 5 };

        root.Children.Add(child1);
        root.Children.Add(child2);
        child1.Children.Add(grandchild1);
        child2.Children.Add(grandchild2);

        var collectedValuesMatching = new List<int>();
        var collectedValuesNonMatching = new List<int>();

        // Act
        TreeHelper.DoActionOnCondition(
            root,
            node => node.Children,
            node => node.Value % 2 == 0, // Condition: Node value is even
            node => collectedValuesMatching.Add(node.Value),
            node => collectedValuesNonMatching.Add(node.Value) // Else action
        );

        // Assert
        collectedValuesMatching.Should().BeEquivalentTo(new List<int> { 2, 4 });
        collectedValuesNonMatching.Should().BeEquivalentTo(new List<int> { 1, 3, 5 });
    }

    [Fact]
    public void GetTree_ShouldBuildTreeCorrectly_FromFlatList()
    {
        // Arrange
        var nodes = new List<FlatNode>
        {
            new FlatNode { Id = 1, ParentId = null },
            new FlatNode { Id = 2, ParentId = 1 },
            new FlatNode { Id = 3, ParentId = 1 },
            new FlatNode { Id = 4, ParentId = 2 },
            new FlatNode { Id = 5, ParentId = 2 }
        };

        // Act
        var result = TreeHelper.GetTree<int, FlatNode>(
            1,
            nodes,
            node => node.Id,
            node => node.ParentId,
            node => node.Children
        );

        // Assert
        result.Should().NotBeNull();
        if (result != null)
        {
            result.Id.Should().Be(1);
            result.Children.Should().HaveCount(2);
            result.Children[0].Id.Should().Be(2);
            result.Children[1].Id.Should().Be(3);
            result.Children[0].Children.Should().HaveCount(2);
            result.Children[0].Children[0].Id.Should().Be(4);
            result.Children[0].Children[1].Id.Should().Be(5);
        }
    }

    #region private


    private class TreeNode
    {
        public int Value { get; set; }
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();
    }

    // Helper class to simulate a flat list of nodes
    private class FlatNode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public List<FlatNode> Children { get; set; } = new List<FlatNode>();
    }

    #endregion
}
