namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    public enum PaymentGatewayTransactionType
    {
        AuthCapture = 1,
        AuthOnly = 2,
        CaptureOnly = 3,
        Credit = 4,
        Void = 5,
        PriorAuthCapture = 6
    }

}
