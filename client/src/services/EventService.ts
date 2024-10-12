export const connectionHandler = (isConnected: boolean) => {
  const event = new CustomEvent<boolean>("connectionUpdate", {
    detail: isConnected,
  });
  window.dispatchEvent(event);
};
