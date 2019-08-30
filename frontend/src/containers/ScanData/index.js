import React, { Component } from "react";
import { connect } from 'react-redux';
import { CircularProgress } from 'components/uielements/progress';
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from 'components/uielements/checkbox';
import {
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from 'components/uielements/table';
import { scanDataActions } from 'redux/actions';
import { Date, Time } from 'helpers/moment';
import _ from 'lodash';
import "styles/style.css";

class ScanData extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true
        }
    }
    componentDidMount() {
        this.props.getScanData(this.onSuccess, this.onSuccess)
    }
    onSuccess = () => {
        this.setState({
            loading: false
        })
    }
    renderData = () => {
        return _.map(this.props.datas, item => (
            <TableRow>
                <TableCell padding="checkbox"><Checkbox /></TableCell>
                <TableCell>{Date(item.createdOn)}</TableCell>
                <TableCell>{Time(item.createdOn)}</TableCell>
                <TableCell>{item.dataType}</TableCell>
                <TableCell>{item.status}</TableCell>
            </TableRow>
        ))
    }
    render() {
        if (this.state.loading) {
            return <CircularProgress />;
        }
        return (
            <LayoutWrapper>
                <FullColumn>
                    <Papersheet title="Users of company CSBG">
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell padding="checkbox"><Checkbox /></TableCell>
                                    <TableCell>Date</TableCell>
                                    <TableCell>Time</TableCell>
                                    <TableCell>Data</TableCell>
                                    <TableCell>Status</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {this.renderData()}
                            </TableBody>
                        </Table>
                        <Grid
                            container
                            direction="row"
                            justify="center"
                            alignItems="center"
                        >
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                                onClick={this.handleAddUser}
                            >
                                Add new user
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                            >
                                Edit selected
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                            >
                                Delete selected
                            </Button>
                        </Grid>
                    </Papersheet>
                </FullColumn>
            </LayoutWrapper>
        );
    }
}
const mapSateToProps = state => {

    return {
        datas: state.ScanData.list
    };
};

const mapDispatchToProps = {
    getScanData: scanDataActions.getScanData
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(ScanData);