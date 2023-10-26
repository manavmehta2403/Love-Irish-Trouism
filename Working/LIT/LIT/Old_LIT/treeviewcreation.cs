using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static LIT.Old_LIT.TreeViewCreation;

namespace LIT.Old_LIT
{
    public class TreeViewCreation
    {




        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged(string propname)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propname));
                }
            }
        }



        public class TreeViewModel : ViewModelBase
        {

            SQLDataAccessLayer.DAL.ItineraryDAL objitdaltrmdl = new SQLDataAccessLayer.DAL.ItineraryDAL();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
            DataSet dsItineraryRetrmdl = new DataSet();
            DataTable dtTree = new DataTable();
            public TreeViewModel()
            {
                dsItineraryRetrmdl = objitdaltrmdl.ItineraryRetrive("R", Guid.Empty);
                if (dsItineraryRetrmdl != null && dsItineraryRetrmdl.Tables.Count > 0)
                {
                    dtTree = dsItineraryRetrmdl.Tables[0];
                }
                BuildTree(dtTree);
            }

            private TreeViewModel _Tree;
            public TreeViewModel Tree
            {
                get { return _Tree; }
                set
                {
                    _Tree = value;
                    OnPropertyChanged("Tree");
                }

            }

            private ObservableCollection<NodeViewModel> _Items;
            public ObservableCollection<NodeViewModel> Items
            {
                get { return _Items; }
                set
                {
                    _Items = value;
                    OnPropertyChanged("Items");
                }
            }

            //public TreeViewModel(DataTable dt)
            //{
            //    BuildTree(dt);
            //}
            //public List<string> fnameTree
            //{
            //    get;set;
            //}
            //public List<string> itname
            //{
            //    get;set;
            //}

           private void BuildTree(DataTable objdt)
            {
               

                 
                string Foldersettingurl = string.Empty, drFoldersettingurl = string.Empty;
                //DataSet dsFolder = new DataSet();
                //dsFolder = objitdaltrmdl.LoadCommonValues("ItineraryFolder");

                //if (dsFolder != null && dsFolder.Tables.Count > 0)
                //{
                //    if (dsFolder.Tables[0].Rows.Count > 0)
                //        Foldersettingurl = dsFolder.Tables[0].Rows[0]["FieldValue"].ToString();
                //}
                //if (Items != null)
                //{ //Items.Clear();
                //    CollectionViewSource.GetDefaultView(this.Items).Refresh();
                //}

                Foldersettingurl = loadDropDownListValues.LoadFolderName("ItineraryFolder");

                Items = new ObservableCollection<NodeViewModel>();

                Image RootParentImage = new Image();
                //  BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/box-green.png"));
                BitmapImage RootParentbitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
                RootParentImage.Source = RootParentbitmapImage;
                var rootNode = new NodeViewModel
                {
                    Name = "Itineraries",
                    Children = new ObservableCollection<NodeViewModel>(),
                    Folderpath = Foldersettingurl,
                    FolderName = "Itineraries",
                    Imageurl = RootParentbitmapImage



                };
                Items.Add(rootNode);

                if (objdt != null && objdt.Rows.Count > 0)
                {
                    DataSet distinctItineraryFolder = new DataSet();
                    distinctItineraryFolder = objitdaltrmdl.GetDbvalue("", "GetItinearyDistinctFolderPath");

                    var lastchildnode = new NodeViewModel();

                    IEnumerable<string> FolderList = GetNameList(objdt.Rows, 2);

                    foreach (DataRow row in distinctItineraryFolder.Tables[0].Rows)
                    {
                        if (row["ItineraryFolderPath"] != null)
                        {
                            //foreach (string FName in FolderList)
                            //{
                            // if (row["ItineraryFolderPath"].ToString().ToLower() == FName.ToLower())
                            // {

                            /*  drFoldersettingurl=row["ItineraryFolderPath"].ToString().Replace(Foldersettingurl, "");
                              if (drFoldersettingurl.Contains("\\"))
                              {
                                  int cnt = drFoldersettingurl.Split("\\").Length;
                                  if (cnt > 0)
                                  {
                                      for (int k=1;k<=cnt; k++)
                                      {
                                          if (!string.IsNullOrEmpty(drFoldersettingurl.Split("\\")[k - 1].ToString()))
                                          {
                                              if (!CheckfolderNameexist(drFoldersettingurl.Split("\\")[k - 1]))
                                              {
                                                  var dbNode = new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k - 1].ToString(),Name = drFoldersettingurl.Split("\\")[k - 1].ToString() };
                                                  if (k == 1) //first one is parent 
                                                  {
                                                      rootNode.Children.Add(dbNode);
                                                  }
                                                  if (k > 1) //next next one are childs
                                                  {
                                                      rootNode.Children.Add(dbNode);
                                                      // dbNode.Children.Add(new NodeViewModel { Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString()  });
                                                      if (k == cnt)
                                                      {
                                                          dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() });
                                                      }
                                                  }
                                              }
                                              else
                                              {
                                                  if (k == cnt)
                                                  {
                                                      var dbNode = new NodeViewModel();
                                                      dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k-1], Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() });
                                                  }
                                                  else
                                                  {
                                                      var dbNode = new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k].ToString(), Name = drFoldersettingurl.Split("\\")[k].ToString() };
                                                      rootNode.Children.Last().Children.Add(dbNode);
                                                      //rootNode.Children[k].Children.Add(dbNode);
                                                      dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k].ToString(), Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() });                                                
                                                  }
                                              }


                                           }

                                      }

                                      // DataTable table = connection.GetSchema("Tables");
                                     // IEnumerable<string> itNamelists = GetNameList(objdt.Rows, 1);

                                      //var tableNode = new NodeViewModel { Name = "Tables" };
                                      //dbNode.Children.Add(tableNode);
                                     // foreach (string itNamelistitm in itNamelists)
                                    //  {

                                    //  }
                                  }
                              }
                              else
                              {
                          //var dbNode = new NodeViewModel { Name = drFoldersettingurl };
                          //rootNode.Children.Add(dbNode);

                          // DataTable table = connection.GetSchema("Tables");
                          //  IEnumerable<string> itNamelists = GetNameList(objdt.Rows, 1);

                          //var tableNode = new NodeViewModel { Name = "Tables" };
                          //dbNode.Children.Add(tableNode);
                          //  foreach (string itNamelistitm in itNamelists)
                          //  {
                          //  rootNode.Children.Add(new NodeViewModel { Name = itNamelistitm, Id = objdt.Rows[0]["ItineraryID"].ToString() });
                          //}
                          if (!string.IsNullOrEmpty(drFoldersettingurl))
                          {
                              if (!CheckfolderNameexist(drFoldersettingurl))
                              {
                                  var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = drFoldersettingurl.ToString() };
                                  rootNode.Children.Add(dbNode);
                                  dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() });

                              }
                              else
                              {
                                  var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() };
                                  rootNode.Children.Add(dbNode);

                              }
                          }
                          else
                          {
                              var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() };
                              rootNode.Children.Add(dbNode);
                          }
                      }


    */
                            NodeViewModel dbNode = null;//new NodeViewModel();
                            drFoldersettingurl = row["ItineraryFolderPath"].ToString().Replace(Foldersettingurl, "");
                            if (!string.IsNullOrEmpty(drFoldersettingurl))
                            {
                                var Itinearyfolderlist = drFoldersettingurl.Split("\\").ToList();

                                if (Itinearyfolderlist.Count > 0)
                                {

                                    GetnodebyFolderpath(rootNode, Foldersettingurl + Itinearyfolderlist[0], ref dbNode);
                                    if (dbNode == null)
                                    {
                                        Image ParImage = new Image();
                                        //  BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/box-green.png"));
                                        BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
                                        ParImage.Source = bitmapImage;

                                        dbNode = new NodeViewModel { Imageurl = ParImage.Source, Folderpath = Foldersettingurl + Itinearyfolderlist[0], FolderName = Itinearyfolderlist[0], Name = Itinearyfolderlist[0] };
                                        rootNode.Children.Add(dbNode);
                                    }
                                    Itinearyfolderlist.RemoveAt(0);
                                    fillnode(dbNode, Itinearyfolderlist);


                                }
                            }

                            var path = from rows in objdt.AsEnumerable().Where(c => c.Field<string>("ItineraryFolderPath") == row["ItineraryFolderPath"].ToString())
                                       select new
                                       {
                                           Folderpath = rows.Field<string>("ItineraryFolderPath"),
                                           Name = rows.Field<string>("ItineraryName"),
                                           Id = rows.Field<Guid>("ItineraryID")
                                       };



                            foreach (var itinerary in path)
                            {
                                GetnodebyFolderpath(rootNode, itinerary.Folderpath.ToString(), ref dbNode);
                                Image ChildImage = new Image();
                                //  BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/box-green.png"));
                                BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "box-green"), UriKind.Relative));
                                ChildImage.Source = bitmapImage;
                                NodeViewModel nodeitinname = new NodeViewModel
                                {
                                    Imageurl = ChildImage.Source,
                                    FolderName = itinerary.Folderpath.ToString(),
                                    Name = itinerary.Name.ToString(),
                                    Id = itinerary.Id.ToString()
                                };
                                dbNode.Children.Add(nodeitinname);

                            }
                            //  rootNode.Children.Add(dbNode);
                            // return drc.Cast<DataRow>().Select(r => r.ItemArray[index].ToString()).OrderBy(r => r).ToList();
                            //var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" };
                            //rootNode.Children.Add(dbNode);
                            //lastchildnode = dbNode;

                            /* if (drFoldersettingurl.Contains("\\"))
                             {                           
                                 int cnt = drFoldersettingurl.Split("\\").Length;
                                 if (cnt > 0)
                                 {



                                     for (int k = 1; k <= cnt; k++)
                                     {
                                         if (!CheckfolderNameexist(drFoldersettingurl.Split("\\")[k - 1]))
                                         {
                                             var dbNode = new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k - 1].ToString(), Name = drFoldersettingurl.Split("\\")[k - 1].ToString(), Parentflag = "Y" };
                                             //var lastchildnodedet = checklastchild(lastchildnode);
                                             //if (lastchildnodedet.Count>0)
                                             //{
                                             //    lastchildnodedet[0].Children.Last().Children.Add(dbNode);
                                             //    lastchildnode = lastchildnodedet[0].Children.Last().Children.Last();
                                             //    rootNode.Children.Add(lastchildnode);
                                             //}
                                             //if (lastchildnodedet.Count == 0)
                                             //{
                                             //    rootNode.Children.Last().Children.Last().Children.Add(dbNode);
                                             //}
                                             if(rootNode.Children.Count==0)
                                             {
                                                 lastchildnode = rootNode;
                                                 lastchildnode.Children.Add(dbNode);

                                             }
                                              if (rootNode.Children.Last().Id == "" && rootNode.Children.Last().Parentflag == "N" && dbNode.Children.Last().Parentflag == "Y")
                                            // if (lastchildnode.Children.Last().Id == "" && lastchildnode.Children.Last().Parentflag == "N" && dbNode.Children.Last().Parentflag == "Y")
                                             {
                                                 if (lastchildnode.Children.Count == 0)
                                                 {
                                                     lastchildnode.Children.Add(dbNode);
                                                     lastchildnode = lastchildnode.Children.Last();
                                                 }
                                                 if (lastchildnode.Children.Count > 0)
                                                 {
                                                     lastchildnode = lastchildnode.Children.Last();
                                                     lastchildnode.Children.Add(dbNode);
                                                 }
                                             }
                                              if (rootNode.Children.Last().Id == null && rootNode.Children.Last().Parentflag == "Y" && dbNode.Parentflag == "Y")
                                             //if (lastchildnode.Children.Last().Id == null && lastchildnode.Children.Last().Parentflag == "Y" && dbNode.Parentflag == "Y")
                                             {
                                                 if (lastchildnode.Children.Count == 0)
                                                 {
                                                     lastchildnode.Children.Add(dbNode);
                                                     lastchildnode = lastchildnode.Children.Last();
                                                 }
                                                 if (lastchildnode.Children.Count > 0)
                                                 {
                                                     lastchildnode = lastchildnode.Children.Last();
                                                     lastchildnode.Children.Add(dbNode);
                                                 }
                                             }
                                             lastchildnode = dbNode;
                                             rootNode.Children.Add(dbNode);
                                             if (k == cnt)
                                             {
                                                 dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k - 1].ToString(), Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" });
                                             }
                                         }
                                         else
                                         {

                                             var dbNodeExist = new NodeViewModel();
                                             dbNodeExist = checkexistNode(drFoldersettingurl.Split("\\")[k - 1]);
                                             lastchildnode = dbNodeExist;
                                             if (k == cnt)
                                             {
                                                 var dbNode = new NodeViewModel();
                                                 dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k - 1], Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" });
                                                 rootNode.Children.Last().Children.Add(dbNode);
                                                 // lastchildnode = dbNode;

                                             }
                                             else
                                             {
                                                 var dbNode = new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k].ToString(), Name = drFoldersettingurl.Split("\\")[k].ToString(), Parentflag = "Y" };
                                                 // rootNode.Children.Last().Children.Add(dbNode);

                                                 if (rootNode.Children.Count == 0)
                                                 {
                                                    // lastchildnode = rootNode;
                                                     lastchildnode.Children.Add(dbNode);

                                                 }
                                                 //  if (rootNode.Children.Last().Id == "" && rootNode.Children.Last().Parentflag == "N" && dbNode.Children.Last().Parentflag == "Y")
                                                 //if (lastchildnode.Children.Last().Id == "" && lastchildnode.Children.Last().Parentflag == "N" && dbNode.Children.Last().Parentflag == "Y")
                                                 //{
                                                     if (lastchildnode.Children.Count == 0)
                                                     {                                                   
                                                         lastchildnode.Children.Add(dbNode);
                                                        // lastchildnode = lastchildnode.Children.Last();
                                                     }
                                                     if (lastchildnode.Children.Count > 0)
                                                     {
                                                        // lastchildnode = lastchildnode.Children.Last();
                                                         lastchildnode.Children.Add(dbNode);                                                    
                                                     }
                                                 //}
                                                 // if (rootNode.Children.Last().Id == null && rootNode.Children.Last().Parentflag == "Y" && dbNode.Parentflag == "Y")
                                                 //if (lastchildnode.Children.Last().Id == null && lastchildnode.Children.Last().Parentflag == "Y" && dbNode.Parentflag == "Y")
                                                 //{
                                                     //if (lastchildnode.Children.Count == 0)
                                                     //{
                                                     //    lastchildnode.Children.Add(dbNode);
                                                     //    lastchildnode = lastchildnode.Children.Last();
                                                     //}
                                                     //if (lastchildnode.Children.Count > 0)
                                                     //{
                                                     //    lastchildnode = lastchildnode.Children.Last();
                                                     //    lastchildnode.Children.Add(dbNode);
                                                     //}
                                                     //lastchildnode = rootNode.Children.Last();
                                                     //lastchildnode.Children.Add(dbNode);
                                                 //}
                                                 rootNode.Children.Add(dbNode);
                                               //  lastchildnode = dbNode;
                                                 //  lastchildnode = rootNode.Children.Last().Children.Last();
                                                 //var lastchildnodedet = checklastchild(lastchildnode);
                                                 //if (lastchildnodedet != null)
                                                 //{
                                                 //    lastchildnodedet[0].Children.Last().Children.Add(dbNode);

                                                 //    rootNode.Children.Add(lastchildnode);
                                                 //}

                                                 //rootNode.Children[k].Children.Add(dbNode);
                                                 //  dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl.Split("\\")[k].ToString(), Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString() });
                                             }
                                         }
                                     }

                                 }
                             }
                             else
                             {
                                 if (!string.IsNullOrEmpty(drFoldersettingurl))
                                 {
                                     if (!CheckfolderNameexist(drFoldersettingurl))
                                     {
                                         var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = drFoldersettingurl.ToString(), Parentflag = "Y" };
                                         rootNode.Children.Add(dbNode);
                                         dbNode.Children.Add(new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" });
                                         lastchildnode = dbNode;
                                     }
                                     else
                                     {
                                         var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" };
                                         rootNode.Children.Add(dbNode);
                                         lastchildnode = dbNode;

                                     }
                                 }
                                 else
                                 {
                                     var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" };
                                     rootNode.Children.Add(dbNode);
                                     lastchildnode = dbNode;
                                 }
                             }
                             */
                            // }                                                                                  
                            // }
                        }
                        //else
                        //{
                        //    var dbNode = new NodeViewModel { FolderName = drFoldersettingurl, Name = row["ItineraryName"].ToString(), Id = row["ItineraryID"].ToString(), Parentflag = "N" };
                        //    rootNode.Children.Add(dbNode);
                        //    lastchildnode = dbNode;
                        //}

                    }


                }
                
               // trvm.Items = Items;
               // trlist.Add(trvm);
                //return trlist;

            }

            private void GetnodebyFolderpath(NodeViewModel rootNode, string path, ref NodeViewModel outputnode)
            {
                //  NodeViewModel = new NodeViewModel();

                if (rootNode.Folderpath == path)
                {
                    outputnode = rootNode;
                    //  return rootNode;
                }
                //if (rootNode.Children.Count > 0)
                //{
                foreach (var itinerary in rootNode.Children)
                {
                    GetnodebyFolderpath(itinerary, path, ref outputnode);
                    //GetnodebyFolderpath(itinerary, itinerary.Folderpath);
                }
                // }
                // return null;

            }

            private void fillnode(NodeViewModel nvm, List<string> ItineraryFolderNames)
            {
                if (ItineraryFolderNames.Count > 0)
                {
                    NodeViewModel childnode = null;


                    GetnodebyFolderpath(nvm, nvm.Folderpath + "\\" + ItineraryFolderNames[0], ref childnode);
                    if (childnode == null)
                    {
                        Image ParentImage = new Image();
                        //BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/folder.png"));
                        BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
                        ParentImage.Source = bitmapImage;

                        childnode = new NodeViewModel
                        {
                            Imageurl = ParentImage.Source,
                            Folderpath = nvm.Folderpath + "\\" + ItineraryFolderNames[0],
                            FolderName = ItineraryFolderNames[0],
                            Name = ItineraryFolderNames[0]
                        };
                        nvm.Children.Add(childnode);
                    }


                    ItineraryFolderNames.RemoveAt(0);
                    fillnode(childnode, ItineraryFolderNames);
                }

            }
            private IEnumerable<string> GetNameList(DataRowCollection drc, int index)
            {
                return drc.Cast<DataRow>().Select(r => r.ItemArray[index].ToString()).OrderBy(r => r).ToList();
            }

            //private IEnumerable<string> GetNameList(DataRowCollection drc)
            //{
            //    return drc.Cast<DataRow>().Select(r => r.ItemArray["ItineraryName"].ToString()).OrderBy(r => r).ToList();
            //}

            NodeViewModel nodeViewModel = new NodeViewModel();
            List<NodeViewModel> nodes = new List<NodeViewModel>();
            private List<NodeViewModel> checklastchild(NodeViewModel nt)
            {
                if (nt.Children.Count > 0)
                {
                    var itemchild = nt.Children.Last();
                    while (itemchild.Children.Count > 0)
                    {
                        foreach (var child in itemchild.Children)
                        {
                            if (child.Children.Count == 0)
                            {
                                nodeViewModel.Children.Add(child);
                                nodes.Add(nodeViewModel);
                                return nodes;
                            }
                            else
                            {
                                checklastchild(child.Children.Last());
                            }
                        }
                    }
                }
                return nodes;
            }

            private NodeViewModel checkexistNode(string drFoldername)
            {

                NodeViewModel nvm = new NodeViewModel();
                if (Items.Count > 0)
                {
                    foreach (var item in Items[0].Children)
                    {
                        if (item.Children.Count > 0)
                        {
                            foreach (var item1 in item.Children)
                            {
                                if (item1.FolderName.Equals(drFoldername))
                                {
                                    nvm.Id = item.Id;
                                    nvm.FolderName = item.FolderName;
                                    nvm.Parentflag = item.Parentflag;
                                    nvm.Children = item.Children;
                                }
                            }
                        }
                        else
                        {
                            if (item.Name.Equals(drFoldername))
                            {
                                nvm.Id = item.Id;
                                nvm.FolderName = item.FolderName;
                                nvm.Parentflag = item.Parentflag;
                                nvm.Children = item.Children;
                            }
                        }
                    }
                }
                return nvm;
            }
            private bool CheckfolderNameexist(string drFoldername)
            {
                if (Items.Count > 0)
                {
                    foreach (var item in Items[0].Children)
                    {
                        if (item.Children.Count > 0)
                        {
                            foreach (var item1 in item.Children)
                            {
                                if (item1.FolderName.Equals(drFoldername))
                                { return true; }
                            }
                        }
                        else
                        {
                            if (item.Name.Equals(drFoldername))
                            { return true; }
                        }
                    }
                }
                return false;
            }
        }

     /*   public class NodeViewModel : ViewModelBase
        {
            public NodeViewModel()
            {
                Children = new ObservableCollection<NodeViewModel>();
            }


            private string _Id;
            public string Id
            {
                get { return _Id; }
                set
                {
                    _Id = value;
                    OnPropertyChangedNotify("Id");
                }
            }

            private string _Parentflag;
            public string Parentflag
            {
                get { return _Parentflag; }
                set
                {
                    _Parentflag = value;
                    OnPropertyChangedNotify("Parentflag");
                }
            }

            private string _Name = string.Empty;
            public string Name
            {
                get { return _Name; }
                set
                {
                    if (_Name != value)
                    {
                        _Name = value;

                        OnPropertyChangedNotify("Name");
                    }
                }
            }

            private string _FolderName = string.Empty;
            public string FolderName
            {
                get { return _FolderName; }
                set
                {
                    if (_FolderName != value)
                    {
                        _FolderName = value;
                        OnPropertyChangedNotify("FolderName");
                    }
                }
            }

            private string _Folderpath = string.Empty;
            public string Folderpath
            {
                get { return _Folderpath; }
                set
                {
                    if (_Folderpath != value)
                    {
                        _Folderpath = value;
                        OnPropertyChangedNotify("Folderpath");
                    }
                }
            }



            //private ImageSource _ChildImageurl = null;
            //public ImageSource ChildImageurl
            //{
            //    get { return _ChildImageurl; }
            //    set
            //    {
            //        if (_ChildImageurl != value)
            //        {
            //            _ChildImageurl = value;
            //            OnPropertyChangedNotify("ChildImageurl");
            //        }
            //    }
            //}


            private ImageSource _Imageurl = null;
            public ImageSource Imageurl
            {
                get { return _Imageurl; }
                set
                {
                    if (_Imageurl != value)
                    {
                        _Imageurl = value;
                        OnPropertyChangedNotify("Imageurl");
                    }
                }
            }

            //private ObservableCollection<NodeViewModel> _FolderStructure;
            //public ObservableCollection<NodeViewModel> FolderStructure
            //{
            //    get { return _FolderStructure; }
            //    set
            //    {
            //        _FolderStructure = value;
            //    }
            //}



            private ObservableCollection<NodeViewModel> _Children;
            public ObservableCollection<NodeViewModel> Children
            {
                get { return _Children; }
                set
                {
                    _Children = value;
                    OnPropertyChangedNotify("Children");
                }
            }

            public CompositeCollection Itemscoll
            {
                get
                {
                    return new CompositeCollection()
          {
             new CollectionContainer() { Collection = Children },
             // Add other type of collection in composite collection
             // new CollectionContainer() { Collection = OtherTypeSources }
          };
                }
            }

        }
     */

        /* Supplier Tree View Start */
        public class ContactTreeViewModel : ViewModelBase
        {

            SQLDataAccessLayer.DAL.ContactInfoDal objContactdaltrmdl = new SQLDataAccessLayer.DAL.ContactInfoDal();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
            // DataTable dtTree = new DataTable();
            public ContactTreeViewModel()
            {
                List<ContactModel> contacts = objContactdaltrmdl.RetrieveContacts(); // Modify this method to retrieve contacts
                if (contacts != null && contacts.Count > 0)
                {
                    OrganizeContactsByCountry(contacts);
                }
            }

            private void OrganizeContactsByCountry(List<ContactModel> contacts)
            {
                SupplierItems = new ObservableCollection<SupplierNodeViewModel>();
                List<contacttype> ListofContactType = new List<contacttype>();
                ContactInfoDal emailDal = new ContactInfoDal();

                ListofContactType = emailDal.RetrieveContactTypeId();

                // Define the root node for contacts organized by country
                var contactsRootNode = new SupplierNodeViewModel
                {
                    SupplierName = "Contacts",
                    SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                    SupplierImageurl = GetCountryFolderImage() // Add the folder image for countries
                };

                SupplierItems.Add(contactsRootNode);

                // Group contacts by country
                var Client = new SupplierNodeViewModel
                {
                    SupplierName = "Client",
                    SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                    SupplierImageurl = GetCountryFolderImage() // Add the folder image for countries
                };
                contactsRootNode.SupplierChildren.Add(Client);

                var Supplier = new SupplierNodeViewModel
                {
                    SupplierName = "Supplier",
                    SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                    SupplierImageurl = GetCountryFolderImage() // Add the folder image for countries
                };
                contactsRootNode.SupplierChildren.Add(Supplier);

                var LIV_User = new SupplierNodeViewModel
                {
                    SupplierName = "LIV User",
                    SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                    SupplierImageurl = GetCountryFolderImage() // Add the folder image for countries
                };
                contactsRootNode.SupplierChildren.Add(LIV_User);

                var Agent = new SupplierNodeViewModel
                {
                    SupplierName = "Agent",
                    SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                    SupplierImageurl = GetCountryFolderImage() // Add the folder image for countries
                };
                contactsRootNode.SupplierChildren.Add(Agent);

                foreach (var contact in contacts)
                {
                    var contactNode = new SupplierNodeViewModel
                    {
                        SupplierName = $"{contact.ContactFirstName} {contact.ContactLastName}",
                        SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                        SupplierImageurl = GetContactImage(), // Add the folder image for countries
                        SupplierId = contact.ContactId.ToString()
                    };
                    var type = ListofContactType.FirstOrDefault(type => Guid.Parse(type.ContactTypeid) == contact.ContactTypeID)?.ContactTypename;
                    if (type == "Supplier")
                    {
                        Supplier.SupplierChildren.Add(contactNode);

                    }
                    else if (type == "Agents")
                    {
                        Agent.SupplierChildren.Add(contactNode);

                    }
                    else if (type == "LIV User")
                    {
                        LIV_User.SupplierChildren.Add(contactNode);

                    }
                    else if (type == "Clients")
                    {
                        Client.SupplierChildren.Add(contactNode);

                    }
                    else
                    {
                        contactsRootNode.SupplierChildren.Add(contactNode);
                    }
                }
            }

            private BitmapImage GetCountryFolderImage()
            {
                return new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
            }

            private BitmapImage GetContactImage()
            {
                // Add logic to retrieve the folder image for countries
                // For example, load an image from a resource or file
                // Here's an example using a placeholder image:
                return new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "house"), UriKind.Relative));
            }


            private SupplierTreeViewModel _SupplierTree;
            public SupplierTreeViewModel SupplierTree
            {
                get { return _SupplierTree; }
                set
                {
                    _SupplierTree = value;
                    OnPropertyChanged("SupplierTree");
                }

            }

            private ObservableCollection<SupplierNodeViewModel> _SupplierItems;
            public ObservableCollection<SupplierNodeViewModel> SupplierItems
            {
                get { return _SupplierItems; }
                set
                {
                    _SupplierItems = value;
                    OnPropertyChanged("SupplierItems");
                }
            }
        }

        public class SupplierTreeViewModel : ViewModelBase
        {

            SQLDataAccessLayer.DAL.SupplierDAL objSuppldaltrmdl = new SQLDataAccessLayer.DAL.SupplierDAL();
            LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
            DataSet dsSupplierRetrmdl = new DataSet();
            // DataTable dtTree = new DataTable();
            List<SupplierModels> listsup = new List<SupplierModels>();
            public SupplierTreeViewModel()
            {
                listsup = objSuppldaltrmdl.SupplierRetriveFolderinfo("R", Guid.Empty);
                if (listsup != null && listsup.Count > 0)
                {
                    BuildTree(listsup);
                }
           }
    
            private SupplierTreeViewModel _SupplierTree;
            public SupplierTreeViewModel SupplierTree
            {
                get { return _SupplierTree; }
                set
                {
                    _SupplierTree = value;
                    OnPropertyChanged("SupplierTree");
                }

            }



            private ObservableCollection<SupplierNodeViewModel> _SupplierItems;
            public ObservableCollection<SupplierNodeViewModel> SupplierItems
            {
                get { return _SupplierItems; }
                set
                {
                    _SupplierItems = value;
                    OnPropertyChanged("SupplierItems");
                }
            }



            private void BuildTree(List<SupplierModels> listsup)
            {

                string Foldersettingurl = string.Empty, drFoldersettingurl = string.Empty;


                Foldersettingurl = loadDropDownListValues.LoadFolderName("SupplierFolder");

                SupplierItems = new ObservableCollection<SupplierNodeViewModel>();

                Image RootParentImage = new Image();
                //  BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/box-green.png"));
                BitmapImage RootParentbitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
                RootParentImage.Source = RootParentbitmapImage;
                var SupplierrootNode = new SupplierNodeViewModel
                {
                    SupplierName = "Suppliers",
                    SupplierChildren = new ObservableCollection<SupplierNodeViewModel>(),
                    SupplierFolderpath = Foldersettingurl,
                    SupplierFolderName = "Suppliers",
                    SupplierImageurl = RootParentbitmapImage
                };
                SupplierItems.Add(SupplierrootNode);

                if (listsup != null && listsup.Count > 0)
                {
                    // DataSet distinctSupplierFolder = new DataSet();
                    List<string> distinctSupplierFolderlist = new List<string>();
                    distinctSupplierFolderlist = objSuppldaltrmdl.distinctSupplierFolderlist();
                    // distinctSupplierFolder = objSuppldaltrmdl.GetDbvalue("", "GetSupplierDistinctFolderPath");

                    var lastchildnode = new SupplierNodeViewModel();

                    // IEnumerable<string> FolderList = GetNameList(objdt.Rows, 2);
                    if (distinctSupplierFolderlist.Count > 0)
                    {
                        foreach (var rowfolder in distinctSupplierFolderlist)
                        {
                            if (rowfolder != null)
                            {

                                SupplierNodeViewModel SupplierdbNode = null;//new NodeViewModel();
                                drFoldersettingurl = rowfolder.ToString().Replace(Foldersettingurl, "");
                                if (!string.IsNullOrEmpty(drFoldersettingurl))
                                {
                                    var Supplierfolderlist = drFoldersettingurl.Split("\\").ToList();

                                    if (Supplierfolderlist.Count > 0)
                                    {

                                        GetnodebyFolderpath(SupplierrootNode, Foldersettingurl + Supplierfolderlist[0], ref SupplierdbNode);
                                        if (SupplierdbNode == null)
                                        {
                                            Image ParImage = new Image();
                                            //  BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/box-green.png"));
                                            BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
                                            ParImage.Source = bitmapImage;

                                            SupplierdbNode = new SupplierNodeViewModel { SupplierImageurl = ParImage.Source, SupplierFolderpath = Foldersettingurl + Supplierfolderlist[0], SupplierFolderName = Supplierfolderlist[0], SupplierName = Supplierfolderlist[0] };
                                            SupplierrootNode.SupplierChildren.Add(SupplierdbNode);
                                        }
                                        Supplierfolderlist.RemoveAt(0);
                                        fillnode(SupplierdbNode, Supplierfolderlist);


                                    }
                                }

                                //var path = from rows in listsup.Where(c => c.Field<string>("SupplierFolderPath") == rowfolder.ToString())
                                //           select new
                                //           {
                                //               Folderpath = rows.Field<string>("SupplierFolderPath"),
                                //               Name = rows.Field<string>("SupplierName"),
                                //               Id = rows.Field<Guid>("SupplierID")
                                //           };


                                var path = from rows in listsup.Where(c => c.SupplierFolderPath == rowfolder.ToString())
                                           select new
                                           {
                                               Folderpath = rows.SupplierFolderPath,
                                               Name = rows.SupplierName,
                                               Id = rows.SupplierId
                                           };

                                foreach (var Supplier in path)
                                {
                                    GetnodebyFolderpath(SupplierrootNode, Supplier.Folderpath.ToString(), ref SupplierdbNode);
                                    Image ChildImage = new Image();
                                    //  BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/box-green.png"));
                                    BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "house"), UriKind.Relative));
                                    ChildImage.Source = bitmapImage;
                                    SupplierNodeViewModel nodeitinname = new SupplierNodeViewModel
                                    {
                                        SupplierImageurl = ChildImage.Source,
                                        SupplierFolderName = Supplier.Folderpath.ToString(),
                                        SupplierName = Supplier.Name.ToString(),
                                        SupplierId = Supplier.Id.ToString()
                                    };
                                    SupplierdbNode.SupplierChildren.Add(nodeitinname);

                                }
                            }


                        }

                    }
                }

            }

            private void GetnodebyFolderpath(SupplierNodeViewModel rootNode, string path, ref SupplierNodeViewModel outputnode)
            {
                //  NodeViewModel = new NodeViewModel();

                if (rootNode.SupplierFolderpath == path)
                {
                    outputnode = rootNode;
                    //  return rootNode;
                }
                //if (rootNode.Children.Count > 0)
                //{
                foreach (var Supplier in rootNode.SupplierChildren)
                {
                    GetnodebyFolderpath(Supplier, path, ref outputnode);
                    //GetnodebyFolderpath(Supplier, Supplier.Folderpath);
                }
                // }
                // return null;

            }

            private void fillnode(SupplierNodeViewModel nvm, List<string> SupplierFolderNames)
            {
                if (SupplierFolderNames.Count > 0)
                {
                    SupplierNodeViewModel childnode = null;


                    GetnodebyFolderpath(nvm, nvm.SupplierFolderpath + "\\" + SupplierFolderNames[0], ref childnode);
                    if (childnode == null)
                    {
                        Image ParentImage = new Image();
                        //BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Resources/folder.png"));
                        BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "folder"), UriKind.Relative));
                        ParentImage.Source = bitmapImage;

                        childnode = new SupplierNodeViewModel
                        {
                            SupplierImageurl = ParentImage.Source,
                            SupplierFolderpath = nvm.SupplierFolderpath + "\\" + SupplierFolderNames[0],
                            SupplierFolderName = SupplierFolderNames[0],
                            SupplierName = SupplierFolderNames[0]
                        };
                        nvm.SupplierChildren.Add(childnode);
                    }


                    SupplierFolderNames.RemoveAt(0);
                    fillnode(childnode, SupplierFolderNames);
                }

            }
            private IEnumerable<string> GetNameList(DataRowCollection drc, int index)
            {
                return drc.Cast<DataRow>().Select(r => r.ItemArray[index].ToString()).OrderBy(r => r).ToList();
            }
        }

        public class Tabitemdetails : ViewModelBase
        {
            public Tabitemdetails() { }
            private string _tabid;
            public string tabid { get { return _tabid; } set {
                    _tabid = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("tabid"));
                } }
           
            
            public string FilePath { get; set; }
            public string tabfrom { get; set; }


            private string _title;
            public string title
            {
                get { return _title; }
                set
                {
                    _title = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("title"));
                }
            }
            private string _tabName;
            public string tabName
            {
                get { return _tabName; }
                set
                {
                    _tabName = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("tabName"));
                }
            }
            //public TabItem tabItemvalues { get; set; }

            private TabItem _tabItemvalues;
            public TabItem tabItemvalues
            {
                get { return _tabItemvalues; }
                set
                {
                    _tabItemvalues = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                    this.OnPropertyChanged(new PropertyChangedEventArgs("tabItemvalues"));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    //  handler(this, new PropertyChangedEventArgs(e));
                    this.PropertyChanged(this, e);
                }
            }
        }

        /* Supplier Tree View End */
    }
}
