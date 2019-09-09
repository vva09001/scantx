import React from 'react';
import Dialog, {
    DialogActions,
    DialogContent,
    DialogTitle,
    DialogContentText
} from 'components/uielements/dialogs';
import Button from 'components/uielements/button';
import { CircularProgress } from 'components/uielements/progress';

class Alert extends React.Component {
    render() {
        const { status, loading, onClose } = this.props;
        if (loading) {
            return <CircularProgress />
        }
        return (
            <Dialog open={status} onClose={() => onClose(false)}>
                <DialogTitle>{"Remove company"}</DialogTitle>
                <DialogContent>
                    <DialogContentText>Please choose one or more scan data to delete</DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => onClose(false)} color="primary">
                        OK
                    </Button>
                </DialogActions>
            </Dialog>
        )
    }
}

export default Alert;