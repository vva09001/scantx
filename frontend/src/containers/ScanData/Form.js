import React from 'react';
import { connect } from 'react-redux';
import {
  TextField
} from '@material-ui/core';
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle,
} from 'components/uielements/dialogs';
import Button from 'components/uielements/button';
import { CircularProgress } from 'components/uielements/progress';

class Form extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false
    }
  }
  componentDidMount() {
    console.log(this.props.params)
  }
  onClose = () => {
    this.props.onToggle(false)
  }
  render() {
    const { status } = this.props;
    return (
      <Dialog open={status} onClose={this.onClose}>
          <DialogTitle>{"Create a new scan data"}</DialogTitle>
          <DialogContent>
            <div>
              <TextField
                required
                id="payload"
                label="Payload"
                margin="normal"
                fullWidth
              />
            </div>
            <div>
              <TextField
                required
                id="data-type"
                label="Data Type"
                margin="normal"
                fullWidth
              />
            </div>
            <div>
              <TextField
                required
                id="file-name"
                label="File Name"
                margin="normal"
                fullWidth
              />
            </div>
            <div>
              <TextField
                required
                id="status"
                label="Status"
                margin="normal"
                fullWidth
              />
            </div>
          </DialogContent>
          {
            this.state.loading ? (
              <CircularProgress />
            ) : (
              <DialogActions>
                <Button onClick={this.onClose} color="primary">
                  Cancel
                </Button>
                <Button onClick={this.onSubmit} color="primary" autoFocus>
                  Submit
                </Button>
              </DialogActions>
              )
          }
        </Dialog>
    )
  }
}

const mapSateToProps = state => {
  return {
  };
};

const mapDispatchToProps = {
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(Form);