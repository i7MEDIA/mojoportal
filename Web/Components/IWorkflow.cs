// Author:						Kevin Needham
// Created:					    2009-06-23
// Last Modified:     
// 2009-07-23  renamed to IWorkflow and added methods   
// 2009-07-24       
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
namespace mojoPortal.Web
{
    public interface IWorkflow
    {
        void SubmitForApproval();
        void CancelChanges();
        void Approve();
    }
}
