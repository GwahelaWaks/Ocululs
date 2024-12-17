using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class profileVersion
{
	private string _ProfileName;
	private Guid _ProfileID;
	private Guid _ProfileVersionID;
	private Guid _ProcessVersionID;

	public Guid ProcessVersionID
    {
		get { return _ProcessVersionID; }
		set { _ProcessVersionID = value; }
	}
	public Guid ProfileVersionID
    {
		get { return _ProfileVersionID; }
		set { _ProfileVersionID = value; }
	}
	public Guid ProfileID
    {
		get { return _ProfileID; }
		set { _ProfileID = value; }
	}
	public string ProfileName
    {
		get { return _ProfileName; }
		set { _ProfileName = value; }
	}
}
