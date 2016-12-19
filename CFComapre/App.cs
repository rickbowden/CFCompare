﻿using Amazon;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFComapre
{
    public partial class App : Form
    {
                    
        string templatePath1 = "";
        string profileName1 = "";
        string jasonString1 = "";
        string stackName1 = "";
        string templatePath2 = "";
        string profileName2 = "";
        string jasonString2 = "";
        string stackName2 = "";

        const string tab1 = "    ";
        const string tab2 = "            ";
        const string tab3 = "                    ";
        const string tab4 = "                            ";
        const string tab5 = "                                    ";

        CFStack StackOriginal1 = null;
        CFStack StackOriginal2 = null;
        CFStack StackCopy1 = null;
        CFStack StackCopy2 = null;

        bool CompareRemoves = true;
        bool ViewSwitch = false;

        RichTextBox richTextBox1temp = new RichTextBox();
        RichTextBox richTextBox2temp = new RichTextBox();

        Dictionary<int, string> Protocols = Utils.Protocol();

        public App()
        {
            InitializeComponent();

            validation1_LB.Visible = false;
            validation2_LB.Visible = false;
            AWSConfigs.AWSRegion = "eu-west-1";
            PopulateProfileList(profile1_CB);
            PopulateProfileList(profile2_CB);
            SwitchView_BTN.Enabled = false;
            
        }




        private void source1_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceSelection(source1_CB);            
        }
        private void source2_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceSelection(source2_CB);   
        }
        private void sourceSelection(ComboBox cb)
        {
            if (cb.Name.Equals(source1_CB.Name))
            {
                switch (cb.Text)
                {
                    case "Template":
                        templateOrStack1_TB.Enabled = true;
                        templateOrStack1_LB.Text = "Template:";
                        templatePath1 = templateOrStack1_TB.Text;
                        break;
                    case "AWS":
                        templateOrStack1_TB.Enabled = true;
                        templateOrStack1_LB.Text = "Stack:";
                        templateOrStack1_TB.Text = "";
                        break;
                }
            }
            else
            {
                switch (cb.Text)
                {
                    case "Template":
                        templateOrStack2_TB.Enabled = true;
                        templateOrStack2_LB.Text = "Template:";
                        templatePath2 = templateOrStack2_TB.Text;
                        break;
                    case "AWS":
                        templateOrStack2_TB.Enabled = true;
                        templateOrStack2_LB.Text = "Stack:";
                        templateOrStack2_TB.Text = "";
                        break;
                }
            }
        }
        private void PopulateProfileList(ComboBox cb)
        {
            cb.Items.Clear();
            foreach (var item in Amazon.Util.ProfileManager.ListProfiles())
            {
                cb.Items.Add(item.Name);
            }
        }




        ///GO
        private void Go1_BTN_Click(object sender, EventArgs e)
        {            
            Go_Window1();            
        }
        private void Go2_BTN_Click(object sender, EventArgs e)
        {
            Go_Window2();            
        }

        private void Go_Window1()
        {
            if (inputsAreValid(source1_CB, profile1_CB, templateOrStack1_TB, validation1_LB))
            {
                AmazonCloudFormationClient CFclient = null;
                AmazonEC2Client EC2client = null;

                StackOriginal1 = new CFStack();
                StackCopy1 = new CFStack();

                try
                {
                    profileName1 = profile1_CB.Text;
                    if (validateTemplate_CB.Checked || source1_CB.Text == "AWS")
                    {
                        var creds = new StoredProfileAWSCredentials(profileName1);
                        CFclient = new AmazonCloudFormationClient(creds);
                        EC2client = new AmazonEC2Client(creds);
                    }

                    switch (source1_CB.Text)
                    {
                        case "Template":
                            templatePath1 = templateOrStack1_TB.Text.Trim();
                            using (StreamReader sr = new StreamReader(templatePath1))
                            {
                                jasonString1 = sr.ReadToEnd();
                                if (validateTemplate_CB.Checked)
                                {
                                    if (validateTemplate(jasonString1, CFclient))
                                    {
                                        ProcessTemplate(jasonString1, richTextBox1, templatePath1, StackOriginal1);                                        
                                    }
                                }
                                else
                                {
                                    ProcessTemplate(jasonString1, richTextBox1, templatePath1, StackOriginal1);
                                }
                            }
                            break;
                        case "AWS":
                            stackName1 = templateOrStack1_TB.Text.Trim();
                            ProcessLiveStack(stackName1, CFclient, EC2client, richTextBox1, StackOriginal1);
                            break;
                    }
                    profileName1 = profile1_CB.Text.Trim();
                }
                catch (Exception ex)
                {
                    richTextBox1.Text = ex.Message;
                }
                finally
                {
                    if (CFclient != null) { CFclient.Dispose(); }
                    if (EC2client != null) { EC2client.Dispose(); }
                }
            }
        }

        private void Go_Window2()
        {
            if (inputsAreValid(source2_CB, profile2_CB, templateOrStack2_TB, validation2_LB))
            {
                AmazonCloudFormationClient CFclient = null;
                AmazonEC2Client EC2client = null;

                StackOriginal2 = new CFStack();
                StackCopy2 = new CFStack();

                try
                {
                    profileName2 = profile2_CB.Text;
                    if (validateTemplate_CB.Checked || source2_CB.Text == "AWS")
                    {
                        var creds = new StoredProfileAWSCredentials(profileName2);
                        CFclient = new AmazonCloudFormationClient(creds);
                        EC2client = new AmazonEC2Client(creds);
                    }

                    switch (source2_CB.Text)
                    {
                        case "Template":
                            templatePath2 = templateOrStack2_TB.Text.Trim();
                            using (StreamReader sr = new StreamReader(templatePath2))
                            {
                                jasonString2 = sr.ReadToEnd();
                                if (validateTemplate_CB.Checked)
                                {
                                    if (validateTemplate(jasonString2, CFclient))
                                    {
                                        ProcessTemplate(jasonString2, richTextBox2, templatePath2, StackOriginal2);
                                    }
                                }
                                else
                                {
                                    ProcessTemplate(jasonString2, richTextBox2, templatePath2, StackOriginal2);
                                }
                            }
                            break;
                        case "AWS":
                            stackName2 = templateOrStack2_TB.Text.Trim();
                            ProcessLiveStack(stackName2, CFclient, EC2client, richTextBox2, StackOriginal2);
                            break;
                    }
                    profileName2 = profile2_CB.Text.Trim();
                }
                catch (Exception ex)
                {
                    richTextBox2.Text = ex.Message;
                }
                finally
                {
                    if (CFclient != null) { CFclient.Dispose(); }
                    if (EC2client != null) { EC2client.Dispose(); }
                }
            }
        }

        private void compare_BTN_Click(object sender, EventArgs e)
        {            
        }

        private void ProcessTemplate(string jsonString, RichTextBox rtb, string path, CFStack stack)
        {
            //Stack From Template
            
            var template = JsonConvert.DeserializeObject<dynamic>(jsonString);
            stack.Description = template.Description;
            //Process StackResources            
            AWSUtils.ProcessTemplateResources(template.Resources, stack);
            stack.Resources.Sort((a, b) => a.LogicalId.CompareTo(b.LogicalId));
            
            WriteOutput(stack, rtb, source1_CB.Text, templateOrStack1_TB.Text);
            
        }


        private void ProcessLiveStack(string stackName, AmazonCloudFormationClient cfClient, AmazonEC2Client ec2Client, RichTextBox rtb, CFStack stack)
        {            
            //Get Live Stack
            DescribeStacksRequest cfRequest = new DescribeStacksRequest();
            cfRequest.StackName = stackName;
            DescribeStacksResponse liveStack = cfClient.DescribeStacks(cfRequest);
            stack.Description = liveStack.Stacks[0].Description;
            
            //Get Stack Resouces
            DescribeStackResourcesRequest cfResourcesRequest = new DescribeStackResourcesRequest();
            cfResourcesRequest.StackName = stackName;
            DescribeStackResourcesResponse liveStackResources = cfClient.DescribeStackResources(cfResourcesRequest);

            Dictionary<string, string> secGroupMap = new Dictionary<string, string>();
            var x = liveStackResources.StackResources.Find(n => n != null && n.ResourceType == "AWS::EC2::SecurityGroup");
            if (x != null)
            {
                //Get SecurityGroups and map id to name
                
                DescribeSecurityGroupsRequest secGroupRequestAll = new DescribeSecurityGroupsRequest();
                
                //Get all security group Id's and cf logicalId's (if any)
                DescribeSecurityGroupsResponse secGroupResponseAll = ec2Client.DescribeSecurityGroups(secGroupRequestAll);
                foreach (SecurityGroup sg in secGroupResponseAll.SecurityGroups)
                {
                    string value = "none";
                    foreach (Amazon.EC2.Model.Tag tag in sg.Tags)
                    {
                        if (tag.Key.Contains("aws:cloudformation:logical-id")) { value = tag.Value; }
                    }
                    secGroupMap.Add(sg.GroupId, value);
                }
            }

            foreach (StackResource liveStackResource in liveStackResources.StackResources)
            {
                switch (liveStackResource.ResourceType)
                {
                    case "AWS::EC2::SecurityGroup":
                        AWSUtils.ProcessEC2SecurityGroupFromAWS(liveStackResource, stack, ec2Client, secGroupMap);
                        break;
                    case "AWS::EC2::NetworkAcl":
                        AWSUtils.ProcessNetworkAclFromAWS(liveStackResource, stack, ec2Client);
                        break;
                    default:
                        break;
                }

            }
            stack.Resources.Sort((a, b) => a.LogicalId.CompareTo(b.LogicalId));

            WriteOutput(stack, rtb, source1_CB.Text, templateOrStack1_TB.Text);
        }



        private void WriteOutput(CFStack s, RichTextBox rtb, string source, string name)
        {
            rtb.Clear();

            rtb.AppendText("Source: " + source); rtb.AppendText(Environment.NewLine);
            rtb.AppendText("Name/Path: " + name); rtb.AppendText(Environment.NewLine); rtb.AppendText(Environment.NewLine);

            rtb.AppendText("Decription: " + s.Description); rtb.AppendText(Environment.NewLine);

            rtb.AppendText("Resources"); rtb.AppendText(Environment.NewLine);
            foreach (var resource in s.Resources)
            {                
                rtb.AppendText(tab1) ; rtb.AppendText(resource.LogicalId); rtb.AppendText(Environment.NewLine);
                rtb.AppendText(tab2); rtb.AppendText("Type: " + resource.Type); rtb.AppendText(Environment.NewLine);
                
                var properties = resource.Properties;
                string type = resource.Type;
                switch (type)
                {
                    case "AWS::EC2::SecurityGroup":
                        EC2SecurityGroup group = (EC2SecurityGroup)resource;
                        rtb.AppendText(tab3); rtb.AppendText("Group Description: " + group.Properties.GroupDescription); rtb.AppendText(Environment.NewLine);
                        var rules = group.Properties.SecurityGroupIngress.OrderBy(a => a.IpProtocol).ThenBy(a => a.ToPort).ThenBy(a => a.FromPort).ThenBy(a => a.CidrIp).ThenBy(a => a.SourceSecurityGroupId);
                        foreach (var rule in rules)
                        {
                            if (rule.State == null)
                            {
                                rtb.AppendText(tab4); rtb.AppendText("Protocol: " + rule.IpProtocol + " | ");
                            }
                            else
                            {
                                rtb.AppendText(rule.State + "       "); rtb.AppendText(tab3); rtb.AppendText("Protocol: " + rule.IpProtocol + " | ");
                            }
                            if (rule.FromPort.Equals(rule.ToPort, StringComparison.Ordinal))
                            {
                                rtb.AppendText("Port Range: " + rule.FromPort + " | ");
                            }
                            else
                            {
                                rtb.AppendText("Port Range: " + rule.FromPort + "-" + rule.ToPort + " | ");
                            }

                            if (rule.CidrIp != null)
                            {
                                rtb.AppendText("Source: " + rule.CidrIp);
                            }
                            else
                            {
                                rtb.AppendText("Source: " + rule.SourceSecurityGroupId);
                            }

                            rtb.AppendText(Environment.NewLine);
                        }
                        break;
                    case "AWS::EC2::NetworkAcl":
                        NetworkAcl acl = (NetworkAcl)resource;
                        rtb.AppendText(tab3); rtb.AppendText("VpcId: " + acl.Properties.VpcId); rtb.AppendText(Environment.NewLine);
                        var aclEntry = acl.Properties.NetworkAclEntry.OrderBy(a => a.Egress).ThenBy(a => a.RuleNumber);
                        bool egressDisplayed = false;
                        bool ingressDisplayed = false;
                        foreach (var rule in aclEntry)
                        {
                            //Rule #, Type, Protocol, Port Range, Source, Allow/Deny
                            if (rule.Egress == false && ingressDisplayed == false)
                            {
                                rtb.AppendText(tab4); rtb.AppendText("Inbound Rules"); rtb.AppendText(Environment.NewLine);
                                ingressDisplayed = true;
                            }
                            else if (rule.Egress == true && egressDisplayed == false)
                            {
                                rtb.AppendText(tab4); rtb.AppendText("Outbound Rules"); rtb.AppendText(Environment.NewLine);
                                egressDisplayed = true;
                            }
                            rtb.AppendText(tab5); rtb.AppendText("Rule: " + rule.RuleNumber + " | ");
                            rtb.AppendText("Protocol: " + Protocols[rule.Protocol] + " (" + rule.Protocol + ") | ");                
                            if (rule.FromPort.Equals(rule.ToPort, StringComparison.Ordinal))
                            {
                                rtb.AppendText("Port Range: " + rule.FromPort + " | ");
                            }
                            else
                            {
                                rtb.AppendText("Port Range: " + rule.FromPort + "-" + rule.ToPort + " | ");
                            }
                            rtb.AppendText("Source: " + rule.CidrBlock + " | ");
                            rtb.AppendText("Allow/Deny: " + rule.RuleAction.ToUpper());
                            rtb.AppendText(Environment.NewLine);
                        }
                        break;
                }

                rtb.AppendText(Environment.NewLine);
            }
            for (int i = 0; i < rtb.Lines.Length; i++)			
            {
                if (rtb.Lines[i].Contains("*"))
                {
                    int selectionStart = rtb.GetFirstCharIndexFromLine(i);
                    int selectionEnd = selectionStart + rtb.Width;
                    rtb.SelectionBackColor = System.Drawing.Color.Yellow;
                    rtb.SelectionStart = selectionStart;
                    rtb.SelectionLength = rtb.Width;

                }
            }
            
        }



        private bool inputsAreValid(ComboBox source, ComboBox profile, TextBox templateOrStack, Label validation)
        {
            bool result = false;

            if (source.Text == null || source.Text == "" || source.Text == "select")
            {
                validation.Text = "Please select a source";
                validation.ForeColor = System.Drawing.Color.Red;
                validation.Visible = true;
            }
            else
            {
                if ((validateTemplate_CB.Checked) && (profile.Text == null || profile.Text == "" || profile.Text == "select"))
                {
                    validation.Text = "Please select profile";
                    validation.ForeColor = System.Drawing.Color.Red;
                    validation.Visible = true;
                }
                else
                {                    
                    if (source.Text == "AWS")
                    {
                        if (templateOrStack.Text == null || templateOrStack.Text == "")
                        {
                            validation.Text = "Please enter a stack name";
                            validation.ForeColor = System.Drawing.Color.Red;
                            validation.Visible = true;
                        }
                        else
                        {
                            result = true;
                            validation1_LB.Text = "";
                        }
                    }
                    else
                    {
                        if (templateOrStack.Text == null || templateOrStack.Text == "")
                        {
                            validation.Text = "Please enter a template path";
                            validation.ForeColor = System.Drawing.Color.Red;
                            validation.Visible = true;
                        }
                        else
                        {
                            result = true;
                            validation1_LB.Text = "";
                        }
                    }
                }
            }

            return result;
        }

        private bool validateTemplate(string input, AmazonCloudFormationClient cfClient)
        {
            bool result = false;
            if (AWSUtils.IsTemplateValid(input, ref cfClient))
            {
                validation1_LB.Text = "Template is valid";
                validation1_LB.ForeColor = System.Drawing.Color.DarkGreen;
                validation1_LB.Visible = true;
                result = true;
            }
            else
            {
                validation1_LB.Text = "Template is not valid";
                validation1_LB.ForeColor = System.Drawing.Color.Red;
                validation1_LB.Visible = true;
            }
            return result;
        }




        private void compare_BTN_Click_1(object sender, EventArgs e)
        {
            if (StackOriginal1 == null || StackOriginal2 == null) { return; }
            
            ClearRTBBackgroundColour(richTextBox1);
            ClearRTBBackgroundColour(richTextBox2);
            CompareRemoves = toolStripMenuItem_CompareRemove.Checked;

            StackCopy1 = StackOriginal1.DeepClone();
            StackCopy2 = StackOriginal2.DeepClone();
            
            CompareStacks();
            ViewSwitch = true;
            SwitchView_BTN.Enabled = true;
        }

        private void CompareStacks()
        {
            CompareStackDescription();
            CompareStackResources();

            WriteOutput(StackCopy1, richTextBox1, source1_CB.Text, templateOrStack1_TB.Text);
            WriteOutput(StackCopy2, richTextBox2, source2_CB.Text, templateOrStack2_TB.Text);
        }

        private void CompareStackDescription()
        {

        }

        private void CompareStackResources()
        {
            if (StackCopy1 == null || StackCopy2 == null) { return; }
            if (StackOriginal1 == null || StackOriginal2 == null) { return; }

            //var compareStack1Resources = CompareStack1.Resources;
                        
            foreach (var stack1Resource in StackOriginal1.Resources)
            {
                var stack1LogicalId = stack1Resource.LogicalId;
                //Find matching resource in Stack2
                var stack2Resource = StackOriginal2.Resources.Find(n => n != null && n.LogicalId == stack1LogicalId);
                //Find matching resource in CompareStack1
                var compareStack1Resource = StackCopy1.Resources.Find(n => n != null && n.LogicalId == stack1LogicalId);
                var compareStack2Resource = StackCopy2.Resources.Find(n => n != null && n.LogicalId == stack1LogicalId);
                if (stack2Resource != null) //Found Matching Resource
                {
                    switch ((String)stack1Resource.Type)
                    {
                        case "AWS::EC2::SecurityGroup":                             
                            CompareSecurityGroups(stack1Resource, stack2Resource, compareStack1Resource, StackCopy1);
                            CompareSecurityGroups(stack2Resource, stack1Resource, compareStack2Resource, StackCopy2);
                            break;
                        case "AWS::EC2::NetworkAcl":                            
                            CompareNetworkAcl(stack1Resource, stack2Resource, compareStack1Resource, StackCopy1);
                            CompareNetworkAcl(stack2Resource, stack1Resource, compareStack2Resource, StackCopy2);
                            break;
                    }
                }
                                
            }
        }


        private void CompareNetworkAcl(NetworkAcl originalResource1, NetworkAcl originalResource2, NetworkAcl copyResource, CFStack copyStack)
        {            
            List<NetworkAclEntry> compareList = copyResource.Properties.NetworkAclEntry;

            foreach (NetworkAclEntry x in originalResource1.Properties.NetworkAclEntry)
            {

                var y = originalResource2.Properties.NetworkAclEntry.Find(n => n != null && n.RuleNumber == x.RuleNumber && n.RuleAction == x.RuleAction && n.Egress.ToString() == x.Egress.ToString() && n.CidrBlock == x.CidrBlock && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.Protocol == x.Protocol && n.NetworkAclId == x.NetworkAclId);
                if (y == null)
                {
                    if (CompareRemoves == false)
                    {
                        var z = copyResource.Properties.NetworkAclEntry.Find(n => n != null && n.RuleNumber == x.RuleNumber && n.RuleAction == x.RuleAction && n.Egress.ToString() == x.Egress.ToString() && n.CidrBlock == x.CidrBlock && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.Protocol == x.Protocol && n.NetworkAclId == x.NetworkAclId);
                        if (z != null)
                        {
                            z.State = "*";
                        }
                    }
                }
                else
                {
                    if (CompareRemoves == true)
                    {
                        var z = copyResource.Properties.NetworkAclEntry.Find(n => n != null && n.RuleNumber == x.RuleNumber && n.RuleAction == x.RuleAction && n.Egress.ToString() == x.Egress.ToString() && n.CidrBlock == x.CidrBlock && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.Protocol == x.Protocol && n.NetworkAclId == x.NetworkAclId);
                        if (z != null)
                        {
                            compareList.Remove(z);
                        }
                    }
                }
            }

            if (copyResource.Properties.NetworkAclEntry.Count() == 0)
            {
                copyStack.Resources.Remove(copyResource);
            }

        }


        private void CompareSecurityGroups(EC2SecurityGroup originalResource1, EC2SecurityGroup originalResource2, EC2SecurityGroup copyResource, CFStack copyStack)
        {            
            List<EC2SecurityGroupIngress> compareList = copyResource.Properties.SecurityGroupIngress;

            foreach (EC2SecurityGroupIngress x in originalResource1.Properties.SecurityGroupIngress)
            {

                var y = originalResource2.Properties.SecurityGroupIngress.Find(n => n != null && n.CidrIp == x.CidrIp && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.IpProtocol == x.IpProtocol && n.SourceSecurityGroupId == x.SourceSecurityGroupId);
                if (y == null)
                {
                    if (CompareRemoves == false) 
                    {
                        var z = copyResource.Properties.SecurityGroupIngress.Find(n => n != null && n.CidrIp == x.CidrIp && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.IpProtocol == x.IpProtocol && n.SourceSecurityGroupId == x.SourceSecurityGroupId);
                        if (z != null)
                        {
                            z.State = "*";
                        }                        
                    }
                }
                else
                {
                    if (CompareRemoves == true) {
                        var z = copyResource.Properties.SecurityGroupIngress.Find(n => n != null && n.CidrIp == x.CidrIp && n.FromPort == x.FromPort && n.ToPort == x.ToPort && n.IpProtocol == x.IpProtocol && n.SourceSecurityGroupId == x.SourceSecurityGroupId);
                        if (z != null)
                        {
                            copyResource.Properties.SecurityGroupIngress.Remove(z);
                        }
                    }
                }
            }

            if (copyResource.Properties.SecurityGroupIngress.Count() == 0)
            {
                copyStack.Resources.Remove(copyResource);
            }

        }

        private void SwitchView_BTN_Click(object sender, EventArgs e)
        {
            ClearRTBBackgroundColour(richTextBox1);
            ClearRTBBackgroundColour(richTextBox2);

            if (ViewSwitch == true)
            {
                WriteOutput(StackOriginal1, richTextBox1, source1_CB.Text, templateOrStack1_TB.Text);
                WriteOutput(StackOriginal2, richTextBox2, source2_CB.Text, templateOrStack2_TB.Text);
                ViewSwitch = false;
            }
            else if (ViewSwitch == false)
            {
                WriteOutput(StackCopy1, richTextBox1, "", "");
                WriteOutput(StackCopy2, richTextBox2, "", "");
                ViewSwitch = true;
            }
        }





        private void toolStripMenuItem_CompareRemove_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_CompareHighlight.Checked = false;
            if (toolStripMenuItem_CompareRemove.Checked == false) { toolStripMenuItem_CompareRemove.Checked = true; }
        }

        private void toolStripMenuItem_CompareHighlight_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_CompareRemove.Checked = false;
            if (toolStripMenuItem_CompareHighlight.Checked == false) { toolStripMenuItem_CompareHighlight.Checked = true; }
        }

        void ClearRTBBackgroundColour(RichTextBox rtb)
        {
            int selectionStart = rtb.GetFirstCharIndexFromLine(0);
            rtb.SelectAll();
            rtb.SelectionBackColor = SystemColors.Window;
        }

    }

    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
