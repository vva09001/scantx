import Tooltip from "@material-ui/core/Tooltip";
import copy from "clipboard-copy";
import * as React from "react";

class CopyToClipboard extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showTooltip: false
    };
  }

  render() {
    return (
      <Tooltip
        open={this.state.showTooltip}
        title={"Copied to clipboard!"}
        leaveDelay={1500}
        onClose={this.handleOnTooltipClose}
        {...(this.props.TooltipProps || {})}
      >
        {this.props.children({ copy: this.onCopy })}
      </Tooltip>
    );
  }

  onCopy = content => {
    copy(content);
    this.setState({ showTooltip: true });
  };

  handleOnTooltipClose = () => {
    this.setState({ showTooltip: false });
  };
}

export default CopyToClipboard;
