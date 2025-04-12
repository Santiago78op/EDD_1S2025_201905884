using AutoGestPro.Core.Nodes;
using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Core.Structures;

/// <summary>
        /// Árbol de Merkle para almacenar y verificar comprobantes
        /// </summary>
        public class MerkleTree : IMerkleTree, IDisposable
        {
            // Nodo raíz del árbol
            public NodeMerkleTree Root { get; set; }
            
            // Diccionario para almacenar los nodos por ID para acceso rápido
            private Dictionary<int, NodeMerkleTree> _nodes;

            /// <summary>
            /// Constructor de la clase
            /// </summary>
            public MerkleTree()
            {
                Root = null;
                _nodes = new Dictionary<int, NodeMerkleTree>();
            }

            /// <summary>
            /// Obtiene el hash raíz del árbol
            /// </summary>
            /// <returns>Hash raíz del árbol</returns>
            public string GetRootHash()
            {
                if (Root == null)
                    return string.Empty;
                
                return Root.Hash;
            }

            /// <summary>
            /// Inserta un nodo en el árbol
            /// </summary>
            /// <param name="id">ID del comprobante</param>
            /// <param name="value">Datos del comprobante</param>
            public void Insert(int id, object value)
            {
                // Validar que el ID sea único
                if (_nodes.ContainsKey(id))
                {
                    Console.WriteLine("La Factura con el ID {0} ya existe.", id);
                    return;
                }
                
                // Crear nuevo nodo
                NodeMerkleTree newNode = new NodeMerkleTree(id, value);
                _nodes.Add(id, newNode);
                
                // Si el árbol está vacío, el nuevo nodo es la raíz
                if (Root == null)
                {
                    Root = newNode;
                    return;
                }
                
                // Agregar el nodo al árbol y recalcular los hashes
                Root = InsertNode(Root, newNode);
                RecalculateHashes(Root);
            }

            /// <summary>
            /// Inserta un nodo en el árbol de forma ordenada
            /// </summary>
            /// <param name="current">Nodo actual</param>
            /// <param name="newNode">Nuevo nodo a insertar</param>
            /// <returns>Nodo actualizado</returns>
            private NodeMerkleTree InsertNode(NodeMerkleTree current, NodeMerkleTree newNode)
            {
                if (current == null)
                    return newNode;
                
                if (newNode.ID < current.ID)
                {
                    current.Left = InsertNode(current.Left, newNode);
                }
                else
                {
                    current.Right = InsertNode(current.Right, newNode);
                }
                
                return current;
            }

            /// <summary>
            /// Recalcula los hashes del árbol a partir de un nodo
            /// </summary>
            /// <param name="node">Nodo a partir del cual recalcular</param>
            private void RecalculateHashes(NodeMerkleTree node)
            {
                if (node == null)
                    return;
                
                // Recalcular hashes de los hijos primero
                if (node.Left != null)
                    RecalculateHashes(node.Left);
                
                if (node.Right != null)
                    RecalculateHashes(node.Right);
                
                // Si es una hoja (nodo sin hijos), ya tiene su hash calculado en el constructor
                if (node.Left == null && node.Right == null)
                    return;
                
                // Si tiene hijos, su hash es el hash de la concatenación de los hashes de sus hijos
                string leftHash = node.Left?.Hash ?? string.Empty;
                string rightHash = node.Right?.Hash ?? string.Empty;
                
                using (SHA256 sha256 = SHA256.Create())
                {
                    string combinedData = leftHash + rightHash;
                    byte[] bytes = Encoding.UTF8.GetBytes(combinedData);
                    byte[] hash = sha256.ComputeHash(bytes);
                    node.Hash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }

            /// <summary>
            /// Elimina un nodo del árbol
            /// </summary>
            /// <param name="id">ID del comprobante a eliminar</param>
            public void Remove(int id)
            {
                if (!_nodes.ContainsKey(id))
                    return;
                
                // Eliminar del diccionario
                _nodes.Remove(id);
                
                // Reconstruir el árbol desde cero
                List<NodeMerkleTree> nodesList = _nodes.Values.ToList();
                Root = null;
                
                foreach (var node in nodesList)
                {
                    if (Root == null)
                        Root = node;
                    else
                        Root = InsertNode(Root, node);
                }
                
                // Recalcular todos los hashes
                if (Root != null)
                    RecalculateHashes(Root);
            }

            /// <summary>
            /// Busca un nodo en el árbol por su ID
            /// </summary>
            /// <param name="id">ID del comprobante a buscar</param>
            /// <returns>Datos del comprobante o null si no se encuentra</returns>
            public object Search(int id)
            {
                if (_nodes.TryGetValue(id, out NodeMerkleTree node))
                    return node.Value;
                
                return null;
            }

            /// <summary>
            /// Modifica un nodo en el árbol
            /// </summary>
            /// <param name="id">ID del comprobante a modificar</param>
            /// <param name="value">Nuevos datos del comprobante</param>
            /// <returns>true si se modificó correctamente, false en caso contrario</returns>
            public bool Modify(int id, object value)
            {
                if (!_nodes.TryGetValue(id, out NodeMerkleTree node))
                    return false;
                
                // Actualizar el valor y recalcular el hash
                node.Value = value;
                node.Hash = ComputeHash(id, value);
                
                // Recalcular todos los hashes
                RecalculateHashes(Root);
                
                return true;
            }

            /// <summary>
            /// Verifica la integridad de los datos
            /// </summary>
            /// <param name="id">ID del comprobante a verificar</param>
            /// <param name="value">Datos del comprobante a verificar</param>
            /// <returns>true si los datos son íntegros, false en caso contrario</returns>
            public bool VerifyData(int id, object value)
            {
                if (!_nodes.TryGetValue(id, out NodeMerkleTree node))
                    return false;
                
                // Verificar si el hash calculado coincide con el almacenado
                string calculatedHash = ComputeHash(id, value);
                return calculatedHash == node.Hash;
            }

            /// <summary>
            /// Calcula el hash de los datos
            /// </summary>
            /// <param name="id">ID del comprobante</param>
            /// <param name="value">Datos del comprobante</param>
            /// <returns>Hash calculado</returns>
            private string ComputeHash(int id, object value)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    string data = id.ToString() + value.ToString();
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    byte[] hash = sha256.ComputeHash(bytes);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }

            /// <summary>
            /// Recorre el árbol en PreOrden
            /// </summary>
            /// <param name="action">Acción a realizar con cada nodo</param>
            public void PreOrder(Action<object> action)
            {
                PreOrder(Root, action);
            }

            /// <summary>
            /// Recorre el árbol en PreOrden de forma recursiva
            /// </summary>
            /// <param name="node">Nodo actual</param>
            /// <param name="action">Acción a realizar con cada nodo</param>
            private void PreOrder(NodeMerkleTree node, Action<object> action)
            {
                if (node != null)
                {
                    action(node.Value);
                    PreOrder(node.Left, action);
                    PreOrder(node.Right, action);
                }
            }

            /// <summary>
            /// Recorre el árbol en InOrden
            /// </summary>
            /// <param name="action">Acción a realizar con cada nodo</param>
            public void InOrder(Action<object> action)
            {
                InOrder(Root, action);
            }

            /// <summary>
            /// Recorre el árbol en InOrden de forma recursiva
            /// </summary>
            /// <param name="node">Nodo actual</param>
            /// <param name="action">Acción a realizar con cada nodo</param>
            private void InOrder(NodeMerkleTree node, Action<object> action)
            {
                if (node != null)
                {
                    InOrder(node.Left, action);
                    action(node.Value);
                    InOrder(node.Right, action);
                }
            }

            /// <summary>
            /// Recorre el árbol en PostOrden
            /// </summary>
            /// <param name="action">Acción a realizar con cada nodo</param>
            public void PostOrder(Action<object> action)
            {
                PostOrder(Root, action);
            }

            /// <summary>
            /// Recorre el árbol en PostOrden de forma recursiva
            /// </summary>
            /// <param name="node">Nodo actual</param>
            /// <param name="action">Acción a realizar con cada nodo</param>
            private void PostOrder(NodeMerkleTree node, Action<object> action)
            {
                if (node != null)
                {
                    PostOrder(node.Left, action);
                    PostOrder(node.Right, action);
                    action(node.Value);
                }
            }

            /// <summary>
            /// Libera los recursos
            /// </summary>
            public void Dispose()
            {
                Root = null;
                _nodes.Clear();
            }

            /// <summary>
            /// Destructor
            /// </summary>
            ~MerkleTree()
            {
                Dispose();
            }
        }