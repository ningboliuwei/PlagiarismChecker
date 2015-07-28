namespace PlagiarismChecker.Utilities
{
	using System;
	using System.Collections.Generic;

	internal class AdjacencyList<T>
	{
		#region Fields

		private readonly List<Vertex<T>> items; //图的顶点集合

		#endregion

		#region Constructors and Destructors

		public AdjacencyList() //构造方法
		{
			this.items = new List<Vertex<T>>();
		}

		#endregion

		#region Public Methods and Operators

		public void AddEdge(T from, T to) //添加无向边
		{
			Vertex<T> fromVer = this.Find(from); //找到起始顶点
			if (fromVer == null)
			{
				throw new ArgumentException("头顶点并不存在！");
			}
			Vertex<T> toVer = this.Find(to); //找到结束顶点
			if (toVer == null)
			{
				throw new ArgumentException("尾顶点并不存在！");
			}
			//无向边的两个顶点都需记录边信息
			this.AddDirectedEdge(fromVer, toVer);
			this.AddDirectedEdge(toVer, fromVer);
		}

		public void AddVertex(T item) //添加一个顶点
		{
			//不允许插入重复值
			//if (this.Contains(item))
			//{
			//	throw new ArgumentException("插入了重复顶点！");
			//}
			this.items.Add(new Vertex<T>(item));
		}

		//查找图中是否包含某项
		public bool Contains(T item)
		{
			foreach (var v in this.items)
			{
				if (v.data.Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			//打印每个节点和它的邻接点
			string s = string.Empty;
			foreach (var v in this.items)
			{
				s += v.data + ":";
				if (v.firstEdge != null)
				{
					Node tmp = v.firstEdge;
					while (tmp != null)
					{
						s += tmp.adjvex.data.ToString();
						tmp = tmp.next;
					}
				}
				s += "\r\n";
			}
			return s;
		}

		#endregion

		#region Methods

		private void AddDirectedEdge(Vertex<T> fromVer, Vertex<T> toVer)
		{
			//无邻接点时
			if (fromVer.firstEdge == null)
			{
				fromVer.firstEdge = new Node(toVer);
			}
			else
			{
				Node tmp, node = fromVer.firstEdge;
				do
				{
					//检查是否添加了重复边
					if (node.adjvex.data.Equals(toVer.data))
					{
						throw new ArgumentException("添加了重复的边！");
					}
					tmp = node;
					node = node.next;
				}
				while (node != null);
				//添加到链表未尾
				tmp.next = new Node(toVer);
			}
		}

		//查找指定项并返回
		private Vertex<T> Find(T item)
		{
			foreach (var v in this.items)
			{
				if (v.data.Equals(item))
				{
					return v;
				}
			}
			return null;
		}

		#endregion

		//相邻结点
		public class Node
		{
			//邻接点

			#region Fields

			public Vertex<T> adjvex;

			//下一个邻接点
			public Node next;

			#endregion

			#region Constructors and Destructors

			public Node(Vertex<T> value)
			{
				this.adjvex = value;
			}

			#endregion
		}

		//顶点
		public class Vertex<TValue>
		{
			//数据

			#region Fields

			public TValue data;

			//邻接点链表头指针
			public Node firstEdge;

			//访问标志,遍历时使用

			public Boolean visited;

			#endregion

			//构造方法

			#region Constructors and Destructors

			public Vertex(TValue value)
			{
				this.data = value;
			}

			#endregion
		}
	}
}