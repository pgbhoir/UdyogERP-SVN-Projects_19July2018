using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MLA.LL;

namespace MLA.APPS
{
    public partial class MLASettings : Form
    {
        public MLASettings()
        {
            InitializeComponent();
        }

        private static string connectionstring;
        public static string Connectionstring
        {
            get { return MLASettings.connectionstring; }
            set { MLASettings.connectionstring = value; }
        }

        private void MLASettings_Load(object sender, EventArgs e)
        {
            m_obj_Settings.GetAllInitialData(0);
            GenerateTranListNodes();
            GenerateRolesNodes();
            // GenerateUserList();
            GeneralLevelNodes();
        }

        cls_Gen_Mgr_Settings m_obj_Settings = new cls_Gen_Mgr_Settings(Connectionstring);
        private bool privateDrag;

        private void GenerateTranListNodes()
        {
            treevwNodes.ImageList = imagelist;
            foreach (DataRow dr in m_obj_Settings.DsSettings.Tables["lcode"].Rows)
            {
                TreeNode trnode = new TreeNode();
                trnode.Name = "Menu";
                trnode.Text = Convert.ToString(dr["code_nm"]).Trim();
                trnode.ImageIndex = 1;
                trnode.SelectedImageIndex = 1;
                treevwNodes.Nodes.Add(trnode);
            }
        }

        private void GenerateRolesNodes()
        {
            treevwUsers.ImageList = imagelist;
            foreach (DataRow dr in m_obj_Settings.DsSettings.Tables["Usersroles"].Rows)
            {
                TreeNode trnode = new TreeNode();
                trnode.Name = "Menu";
                trnode.Text = Convert.ToString(dr["user_roles"]).Trim();
                trnode.Tag = "ROLE";
                trnode.ImageIndex = 2;
                trnode.SelectedImageIndex = 2;
                treevwUsers.Nodes.Add(trnode);

                GenerateUserNodes(trnode, 0);
            }
        }

        private void GenerateUserNodes(TreeNode parent,
                                       int ImageIndex)
        {
            TreeNode Child = null;
            foreach (DataRow dr in m_obj_Settings.DsSettings.Tables["Users"].Select("user_roles = '" + parent.Text + "'"))
            {
                Child = new TreeNode();
                Child.Name = dr["user"] != DBNull.Value ? Convert.ToString(dr["user"]).Trim() : string.Empty;
                Child.Text = dr["user"] != DBNull.Value ? Convert.ToString(dr["user"]).Trim() : string.Empty;
                Child.Tag = "USER";
                Child.ImageIndex = ImageIndex;
                Child.SelectedImageIndex = ImageIndex;

                 parent.Nodes.Add(Child);    
            }

        }

      

        private void GeneralLevelNodes()
        {
            treevwLevels.ImageList = imagelist;
            for (int l = 1; l < 10; l++)
            {
                TreeNode trnode = new TreeNode();
                trnode.Name = "Approval Levels";
                trnode.Text = " Approval Level " + Convert.ToString(l).Trim();
                trnode.Tag = "LEVEL";
                trnode.ImageIndex = 3;
                trnode.SelectedImageIndex = 3;
                treevwLevels.Nodes.Add(trnode);
            }
        }

      
        private void btnUpUsers_Click(object sender, EventArgs e)
        {
            for (int i=0; i< treevwUsers.Nodes.Count; i++)
            {
                TreeNode userchildnode;
                TreeNode roleparentnode;
                if (treevwUsers.Nodes[i].Checked == true)
                {
                    roleparentnode = new TreeNode();
                    roleparentnode.Text = treevwUsers.Nodes[i].Text;
                    roleparentnode.ImageIndex = treevwUsers.Nodes[i].ImageIndex;
                    roleparentnode.SelectedImageIndex = treevwUsers.Nodes[i].SelectedImageIndex;
                    roleparentnode.Tag = treevwUsers.Nodes[i].Tag;

                    foreach (TreeNode usersnode in treevwUsers.Nodes[i].Nodes)
                    {
                        if (usersnode.Checked == true)
                        {
                            userchildnode = new TreeNode();
                            userchildnode.Text = usersnode.Text;
                            userchildnode.ImageIndex = usersnode.ImageIndex;
                            userchildnode.SelectedImageIndex = usersnode.SelectedImageIndex;
                            userchildnode.Tag = usersnode.Tag;

                            roleparentnode.Nodes.Add(userchildnode);

                            usersnode.Remove();  // remove node
                        }
                    }

                    foreach (TreeNode levelssnode in treevwLevels.Nodes)
                    {
                        if (levelssnode.Nodes.Count == 0)
                        {
                            levelssnode.Nodes.Add(roleparentnode);
                            break;
                        }
                    }

                    // Remove parent Role node if child user nodes not exist
                    if (treevwUsers.Nodes[i].Nodes.Count == 0)
                    {
                        treevwUsers.Nodes[i].Remove();
                        i = i - 1;
                    }
                    else
                        treevwUsers.Nodes[i].Checked = false;
                }
            }

            
          
 
        }

        private void btnDownUsers_Click(object sender, EventArgs e)
        {
            foreach (TreeNode nodelevels in treevwLevels.Nodes)
            {
                if (nodelevels.Checked == true)
                {
                    foreach (TreeNode noderoles in nodelevels.Nodes)
                    {
                        TreeNode RolesNodes;
                        if (noderoles.Checked == true)
                        {
                            RolesNodes = new TreeNode();
                            RolesNodes.Text = noderoles.Text;
                            RolesNodes.ImageIndex = noderoles.ImageIndex;
                            RolesNodes.SelectedImageIndex = noderoles.SelectedImageIndex;
                            RolesNodes.Tag = noderoles.Tag;

                            TreeNode UsersNodes;
                            foreach (TreeNode nodeusers in noderoles.Nodes)
                            {
                                if (nodeusers.Checked == true)
                                {
                                    UsersNodes = new TreeNode();
                                    UsersNodes.Text = nodeusers.Text;
                                    UsersNodes.ImageIndex = nodeusers.ImageIndex;
                                    UsersNodes.SelectedImageIndex = nodeusers.SelectedImageIndex;
                                    UsersNodes.Tag = nodeusers.Tag;

                                    RolesNodes.Nodes.Add(UsersNodes);

                                    nodeusers.Remove();
                                }
                            }

                            bool checkExistingNodes = false;
                            foreach (TreeNode nodeusers in treevwUsers.Nodes)
                            {
                                if (nodeusers.Text == RolesNodes.Text)
                                {
                                    checkExistingNodes = true;
                                    foreach (TreeNode nodesroles in RolesNodes.Nodes)
                                    {
                                        nodeusers.Nodes.Add(nodesroles);
                                    }
                                }
                            }

                            if (checkExistingNodes == false)
                                treevwUsers.Nodes.Add(RolesNodes);

                            if (noderoles.Parent != null)
                                noderoles.Parent.Checked = false;

                            if (noderoles.Nodes.Count == 0)
                                noderoles.Remove();

                            
                        }
                    }
                }
            }
        }

        private void lstvwLevels_ItemDrag(object sender, ItemDragEventArgs e)
        {
            
            privateDrag = true;
            DoDragDrop(e.Item, DragDropEffects.Move);
            privateDrag = false;
        }

      

        private void treevwUsers_ItemDrag(object sender, ItemDragEventArgs e)
        {
            string strItem = e.Item.ToString();
            DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void treevwLevels_ItemDrag(object sender, ItemDragEventArgs e)
        {
            string strItem = e.Item.ToString();
            DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void treevwUsers_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treevwLevels_DragEnter(object sender, DragEventArgs e)
        {
            //Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            //TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
            //if (Convert.ToString(DestinationNode.Tag) == "Level")
                e.Effect = DragDropEffects.Move;
        }

        private void treevwLevels_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode;

            //if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            //{
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (DestinationNode.TreeView != NewNode.TreeView)
                {
                    if (Convert.ToString(DestinationNode.Tag) == "LEVEL")
                    {
                        bool isRolesNodeFound = false;
                        if (NewNode.Parent != null)
                        {
                            TreeNode ParentNode = new TreeNode();
                            foreach (TreeNode desinode in DestinationNode.Nodes)
                            {
                                if (desinode.Text.ToUpper().Trim() == NewNode.Parent.Text.ToUpper().Trim())
                                {
                                    isRolesNodeFound = true;
                                    ParentNode = desinode;
                                    break;
                                }
                            }

                            if (isRolesNodeFound == false)
                            {
                                ParentNode = new TreeNode();
                                ParentNode.Text = NewNode.Parent.Text;
                                ParentNode.ImageIndex = NewNode.Parent.ImageIndex;
                                ParentNode.SelectedImageIndex = NewNode.Parent.SelectedImageIndex;
                                ParentNode.Tag = NewNode.Parent.Tag;
                            }

                            foreach (TreeNode node in NewNode.Parent.Nodes)
                            {
                                if (node.Checked == true)
                                {
                                    TreeNode childNode = new TreeNode(node.Text);
                                    ParentNode.Nodes.Add(childNode);

                                  
                                }
                            }

                            if (isRolesNodeFound == false)
                                DestinationNode.Nodes.Add(ParentNode);

                            // DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                            DestinationNode.Expand();
                            //Remove Original Node
                            if (NewNode.Parent.Nodes.Count > 0)
                                NewNode.Remove();
                            else
                                NewNode.Parent.Remove();
                        }
                        else
                        {
                            TreeNode ParentNode = new TreeNode();
                            foreach (TreeNode desinode in DestinationNode.Nodes)
                            {
                                if (desinode.Text.ToUpper().Trim() == NewNode.Text.ToUpper().Trim())
                                {
                                    isRolesNodeFound = true;
                                    ParentNode = desinode;
                                    break;
                                }
                            }

                            if (isRolesNodeFound == false)
                            {
                                ParentNode = new TreeNode();
                                ParentNode.Text = NewNode.Text;
                                ParentNode.ImageIndex = NewNode.ImageIndex;
                                ParentNode.SelectedImageIndex = NewNode.SelectedImageIndex;
                                ParentNode.Tag = NewNode.Tag;
                            }

                            foreach (TreeNode node in NewNode.Nodes)
                            {
                                if (node.Checked == true)
                                {
                                    TreeNode childNode = new TreeNode(node.Text);
                                    ParentNode.Nodes.Add(childNode);

                                    node.Remove();
                                }
                            }

                            if (isRolesNodeFound == false)
                                DestinationNode.Nodes.Add(ParentNode);

                            DestinationNode.Expand();
                            //Remove Original Node
                            if (NewNode.Nodes.Count == 0)
                                NewNode.Remove();
                        }
                    }
                }
                else
                {
                    if (DestinationNode.TreeView == NewNode.TreeView)
                    {
                        if (NewNode.Parent != null) 
                        {
                            if (NewNode.Parent.Tag.ToString().Trim() == "ROLE")
                            {
                                bool isRolesNodeFound = false;
                                TreeNode ParentNode = new TreeNode();
                                foreach (TreeNode desinode in DestinationNode.Nodes)
                                {
                                    if (desinode.Text.ToUpper().Trim() == NewNode.Parent.Text.ToUpper().Trim())
                                    {
                                        isRolesNodeFound = true;
                                        ParentNode = desinode;
                                        break;
                                    }
                                }

                                if (isRolesNodeFound == false)
                                {
                                    ParentNode = new TreeNode();
                                    ParentNode.Text = NewNode.Parent.Text;
                                    ParentNode.ImageIndex = NewNode.Parent.ImageIndex;
                                    ParentNode.SelectedImageIndex = NewNode.Parent.SelectedImageIndex;
                                    ParentNode.Tag = NewNode.Parent.Tag;
                                }

                                foreach (TreeNode node in NewNode.Parent.Nodes)
                                {
                                    if (node.Checked == true)
                                    {
                                        TreeNode childNode = new TreeNode(node.Text);
                                        ParentNode.Nodes.Add(childNode);

                                     
                                    }
                                }

                                if (isRolesNodeFound == false)
                                    DestinationNode.Nodes.Add(ParentNode);

                                // DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                                DestinationNode.Expand();
                                //Remove Original Node
                                if (NewNode.Parent.Nodes.Count > 0)
                                {
                                    if (NewNode.Parent != null)
                                        NewNode.Parent.Checked = false;

                                    NewNode.Remove();
                                    
                                }
                                else
                                    NewNode.Parent.Remove();
                            }
                            else
                            {
                                if (Convert.ToString(DestinationNode.Tag) == "USER")
                                {
                                    return;
                                }

                                bool isRolesNodeFound = false;
                                TreeNode ParentNode = new TreeNode();
                                foreach (TreeNode desinode in DestinationNode.Nodes)
                                {
                                    if (desinode.Text.ToUpper().Trim() == NewNode.Text.ToUpper().Trim())
                                    {
                                        isRolesNodeFound = true;
                                        ParentNode = desinode;
                                        break;
                                    }
                                }

                                if (isRolesNodeFound == false)
                                {
                                    ParentNode = new TreeNode();
                                    ParentNode.Text = NewNode.Text;
                                    ParentNode.ImageIndex = NewNode.ImageIndex;
                                    ParentNode.SelectedImageIndex = NewNode.SelectedImageIndex;
                                    ParentNode.Tag = NewNode.Tag;
                                }

                                foreach (TreeNode node in NewNode.Nodes)
                                {
                                    if (node.Checked == true)
                                    {
                                        TreeNode childNode = new TreeNode(node.Text);
                                        ParentNode.Nodes.Add(childNode);
                                    }
                                }

                                if (isRolesNodeFound == false)
                                    DestinationNode.Nodes.Add(ParentNode);

                                // DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                                DestinationNode.Expand();
                                //Remove Original Node
                                if (NewNode.Parent != null)
                                    NewNode.Parent.Checked = false;

                                if (NewNode.Nodes.Count > 0)
                                    NewNode.Remove();

                             
                                //else
                                //    NewNode.Parent.Remove();
                            }
                        }
                    }
                }
                //}
            //}
        }

        private bool updatingTreeView;
        private void treevwUsers_AfterCheck(object sender, TreeViewEventArgs e)
        {
            label4.Text = "updatingTreeView :" + updatingTreeView.ToString();
            if (updatingTreeView) return;

            updatingTreeView = true;

            CheckNodes(e.Node, e.Node.Checked);
            updatingTreeView = false;
            label4.Text = "updatingTreeView :" + updatingTreeView.ToString();
            
        }

        private void CheckNodes(TreeNode node, Boolean isChecked)
        {
            if (node.Parent != null && node.Nodes.Count == 0)
                SelectParentNodes(node, isChecked);
            else
            {
                if (node.Parent != null && node.Nodes.Count > 0)
                {
                    SelectChildNodes(node.Parent, isChecked);
                    node.Parent.Checked = isChecked;
                }
                else
                    SelectChildNodes(node, isChecked);
            }
        }


        private void SelectChildNodes(TreeNode node, Boolean isChecked)
        {
            foreach (TreeNode nodes in node.Nodes)
            {
                nodes.Checked = isChecked;
                SelectChildNodes(nodes, isChecked);
            }
        }

        private void SelectParentNodes(TreeNode node, Boolean isChecked)
        {
            if (isChecked == false)
            {
                if (node.Parent != null)
                {
                    bool isfound = false;
                    foreach (TreeNode nodes in node.Parent.Nodes)
                    {
                        if (nodes.Checked == true)
                            isfound = true;
                    }
                    if (isfound == false)
                    {

                        node.Parent.Checked = isChecked;
                        SelectParentNodes(node.Parent, isChecked);
                    }
                }
            
            }
            else
                if (node.Parent != null)
                {
                    node.Parent.Checked = true;
                    SelectParentNodes(node.Parent, isChecked);
                }           
        }

       

        
    }
}
