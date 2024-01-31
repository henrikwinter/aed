using Dextra.Toolsspace;
using System.IO;
using System.Xml;

namespace Dextra.Flowbase
{
    public static class Flowutils
    {

        private static void TraverseNodes(XmlNodeList nodes, ref string outv)
        {

            foreach (XmlNode node in nodes)
            {
                // Do something with the node.
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:


                        if (node.ChildNodes != null && node.ChildNodes.Item(0) != null)
                        {
                            if (node.ChildNodes.Count > 1 || node.ChildNodes.Item(0).NodeType == XmlNodeType.Element)
                            {
                                outv += @"<li class=""" + node.Name + @""">" + Langue.Lang(node.Name) + "<ul>";
                                TraverseNodes(node.ChildNodes, ref outv);
                            }
                            else
                            {
                                outv += @"<li>" + node.InnerText + @"</li>";
                            }
                        }
                        else
                        {

                            outv += @"<li>" + node.InnerText + @"</li>";
                        }
                        if (node.NextSibling == null)
                        {
                            outv += @"</ul></li>";
                        }
                        break;
                    case XmlNodeType.Text:
                        outv += "";
                        break;
                    default:
                        outv += "";
                        break;
                }

                //TraverseNodes(node.ChildNodes,ref outv);
            }

        }


        public static string RenderHistory(string xmls)
        {
            string o = "<ul>";
            try
            {
                XmlDocument doc = new XmlDocument();
                StringReader re = new StringReader(xmls);
                doc.Load(re);
                TraverseNodes(doc.ChildNodes[0].ChildNodes, ref o);
                o = o.Substring(0, o.Length - 5);
            }
            catch
            {
                o += "</ul>";
            }

            return o + "";
        }


    }


}