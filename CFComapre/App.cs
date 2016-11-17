using Amazon;
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
        const string tab2 = "        ";
        const string tab3 = "            ";
        const string tab4 = "                ";




        public App()
        {
            InitializeComponent();

            validation1_LB.Visible = false;
            validation2_LB.Visible = false;
            AWSConfigs.AWSRegion = "eu-west-1";
            PopulateProfileList(profile1_CB);
            PopulateProfileList(profile2_CB);
            
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
                                        ProcessTemplate(jasonString1, richTextBox1);
                                    }
                                }
                                else
                                {
                                    ProcessTemplate(jasonString1, richTextBox1);
                                }
                            }
                            break;
                        case "AWS":
                            stackName1 = templateOrStack1_TB.Text.Trim();
                            ProcessLiveStack(stackName1, CFclient, EC2client, richTextBox1);
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
                                        ProcessTemplate(jasonString2, richTextBox2);
                                    }
                                }
                                else
                                {
                                    ProcessTemplate(jasonString2, richTextBox2);
                                }
                            }
                            break;
                        case "AWS":
                            stackName2 = templateOrStack2_TB.Text.Trim();
                            ProcessLiveStack(stackName2, CFclient, EC2client, richTextBox2);
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

        private void ProcessTemplate(string jsonString, RichTextBox rtb)
        {
            //Stack From Template
            CFStack stack = new CFStack();
            var template = JsonConvert.DeserializeObject<dynamic>(jsonString);
            stack.Description = template.Description;
            //Process StackResources            
            AWSUtils.ProcessTemplateResources(template.Resources, stack);
            stack.Resources.Sort((a, b) => a.LogicalId.CompareTo(b.LogicalId));
            foreach (var item in stack.Resources)
            {
                
            }
            WriteOutput(stack, rtb);
            
        }


        private void ProcessLiveStack(string stackName, AmazonCloudFormationClient cfClient, AmazonEC2Client ec2Client, RichTextBox rtb)
        {
            //Stack From AWS
            CFStack runningStack = new CFStack();

            //Get Live Stack
            DescribeStacksRequest cfRequest = new DescribeStacksRequest();
            cfRequest.StackName = stackName;
            DescribeStacksResponse liveStack = cfClient.DescribeStacks(cfRequest);
            runningStack.Description = liveStack.Stacks[0].Description;
            

            //Get Stack Resouces
            DescribeStackResourcesRequest cfResourcesRequest = new DescribeStackResourcesRequest();
            cfResourcesRequest.StackName = stackName;
            DescribeStackResourcesResponse liveStackResources = cfClient.DescribeStackResources(cfResourcesRequest);

            //Get SecurityGroups and map id to name
            Dictionary<string, string> secGroupMap = new Dictionary<string, string>();
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

            foreach (StackResource liveStackResource in liveStackResources.StackResources)
            {
                switch (liveStackResource.ResourceType)
                {
                    case "AWS::EC2::SecurityGroup":
                        AWSUtils.ProcessEC2SecurityGroupFromAWS(liveStackResource, runningStack, ec2Client, secGroupMap);
                        break;
                    default:
                        break;
                }

            }
            runningStack.Resources.Sort((a, b) => a.LogicalId.CompareTo(b.LogicalId));

            WriteOutput(runningStack, rtb);
        }



        private void WriteOutput(CFStack s, RichTextBox rtb)
        {
            rtb.Clear();

            
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
                        var ingressRules = group.Properties.SecurityGroupIngress.OrderBy(a => a.IpProtocol).ThenBy(a => a.ToPort).ThenBy(a => a.FromPort).ThenBy(a => a.CidrIp).ThenBy(a => a.SourceSecurityGroupId);
                        foreach (var ingressRule in ingressRules)
                        {
                            rtb.AppendText(tab4); rtb.AppendText("Protocol: " + ingressRule.IpProtocol + " | ");
                            
                            if (ingressRule.FromPort.Equals(ingressRule.ToPort, StringComparison.Ordinal))
                            {
                                rtb.AppendText("Port Range: " + ingressRule.FromPort + " | ");
                            }
                            else
                            {
                                rtb.AppendText("Port Range: " + ingressRule.FromPort + "-" + ingressRule.ToPort + " | ");
                            }
                            
                            if (ingressRule.CidrIp != null)
                            {
                                rtb.AppendText("Source: " + ingressRule.CidrIp);
                            }
                            else
                            {
                                rtb.AppendText("Source: " + ingressRule.SourceSecurityGroupId);
                            }

                            rtb.AppendText(Environment.NewLine);
                        }
                        break;                    
                }

                rtb.AppendText(Environment.NewLine);
            }
            foreach (var item in rtb.Lines)
            {
                
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

        

        

    }
}
